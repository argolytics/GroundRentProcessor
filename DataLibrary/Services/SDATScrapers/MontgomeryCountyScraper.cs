using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DataLibrary.Services.SDATScrapers;

public class MontgomeryCountyScraper : IRealPropertySearchScraper
{
    private readonly IDataContext _dataContext;
    private readonly MontgomeryCountyDataServiceFactory _MontgomeryCountyDataServiceFactory;
    private IWebElement Input { get; set; }
    private string BaseUrl { get; set; } = "https://sdat.dat.maryland.gov/RealProperty/Pages/default.aspx";
    private int currentCount;
    private int totalCount;
    private decimal percentComplete;
    private int exceptionCount = 0;

    public MontgomeryCountyScraper(
        IDataContext dataContext,
        MontgomeryCountyDataServiceFactory MontgomeryCountyDataServiceFactory)
    {
        _dataContext = dataContext;
        _MontgomeryCountyDataServiceFactory = MontgomeryCountyDataServiceFactory;
    }
    public async Task Scrape(
        RemoteWebDriver remoteWebDriver,
        WebDriverWait webDriverWait,
        List<AddressModel> addressList,
        int amountToScrape)
    {
        using (var uow = _dataContext.CreateUnitOfWork())
        {
            var MontgomeryCountyDataService = _MontgomeryCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
            addressList = await MontgomeryCountyDataService.ReadTopAmountWhereIsGroundRentNull(amountToScrape);
        }
        currentCount = 0;
        totalCount = addressList.Count;
        bool result;
        bool checkingIfAddressExists = true;

        try
        {
            var iterList = addressList.ToList();
            foreach (var address in iterList)
            {
                // Selecting "MONTGOMERY COUNTY"
                remoteWebDriver.Navigate().GoToUrl(BaseUrl);
                Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlCounty > option:nth-child(17)")));
                Input.Click();

                // Selecting "PROPERTY ACCOUNT IDENTIFIER"
                Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlSearchType > option:nth-child(3)")));
                Input.Click();

                // Click Continue button
                Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StartNavigationTemplateContainerID_btnContinue")));
                Input.Click();

                // Input Ward
                Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtDistrict")));
                Input.Clear();
                Input.SendKeys(address.Ward);

                // Input AccountNumber
                Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtAccountIdentifier")));
                Input.Clear();
                Input.SendKeys(address.AccountNumber);

                // Click Next button
                Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StepNavigationTemplateContainerID_btnStepNextButton")));
                Input.Click();
                if (remoteWebDriver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr")).Count != 0)
                {
                    if (remoteWebDriver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr"))
                        .Text.Contains("There are no records that match your criteria"))
                    {
                        // Address does not exist in SDAT
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var MontgomeryCountyDataService = _MontgomeryCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                            result = await MontgomeryCountyDataService.Delete(address.AccountId);
                        }
                        currentCount++;
                        Console.WriteLine($"{address.AccountId.Trim()} does not exist and was deleted.");
                    }
                    else
                    {
                        Console.WriteLine($"{remoteWebDriver} found {address.AccountId.Trim()} does not exist and tried to delete, but the error message text is different than usual: {remoteWebDriver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
                        remoteWebDriver.Quit();
                    }
                }
                else
                {
                    // Click Ground Rent Registration link
                    Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucDetailsSearch_dlstDetaisSearch_lnkGroundRentRegistration_0")));
                    Input.Click();

                    // Condition: check if html has ground rent error tag (meaning property has no ground rent registered)
                    if (remoteWebDriver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Count != 0)
                    {
                        if (remoteWebDriver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr"))
                            .Text.Contains("There is currently no ground rent"))
                        {
                            // Property is not ground rent
                            address.IsGroundRent = false;
                            using (var uow = _dataContext.CreateUnitOfWork())
                            {
                                var MontgomeryCountyDataService = _MontgomeryCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                                result = await MontgomeryCountyDataService.CreateOrUpdateSDATScraper(new AddressModel
                                {
                                    AccountId = address.AccountId,
                                    IsGroundRent = address.IsGroundRent
                                });
                                currentCount++;
                                addressList.Remove(address);
                            }
                            if (result is false)
                            {
                                // Something wrong happened and I do not want the application to skip over this address
                                remoteWebDriver.Quit();
                                Console.WriteLine($"Db could not complete transaction for {address.AccountId.Trim()}. Quitting scrape.");
                            }
                        }
                        else
                        {
                            remoteWebDriver.Quit();
                            Console.WriteLine($"{remoteWebDriver} found {address.AccountId.Trim()} has a different error message than 'There is currently no ground rent' which is: {remoteWebDriver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
                        }
                    }
                    else
                    {
                        // Property must be ground rent
                        address.IsGroundRent = true;
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var MontgomeryCountyDataService = _MontgomeryCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                            result = await MontgomeryCountyDataService.CreateOrUpdateSDATScraper(new AddressModel
                            {
                                AccountId = address.AccountId,
                                IsGroundRent = address.IsGroundRent
                            });
                            currentCount++;
                            addressList.Remove(address);
                        }
                        if (result is false)
                        {
                            // Something wrong happened and I do not want the application to skip over this address
                            remoteWebDriver.Quit();
                            Console.WriteLine($"Db could not complete transaction for {address.AccountId.Trim()}. Quitting scrape.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (exceptionCount > 3)
            {
                remoteWebDriver.Quit();
                Console.WriteLine("Scraper quit. Exception count passed safety threshold.");
            }
            Console.WriteLine(ex.Message);
            exceptionCount++;
            await RestartScrape(remoteWebDriver, webDriverWait, addressList, amountToScrape);
        }
        finally
        {
            await RestartScrape(remoteWebDriver, webDriverWait, addressList, amountToScrape);
        }
    }
    private async Task RestartScrape(
        RemoteWebDriver remoteWebDriver,
        WebDriverWait webDriverWait,
        List<AddressModel> addressList,
        int amountToScrape)
    {
        addressList.Clear();
        using (var uow = _dataContext.CreateUnitOfWork())
        {
            var MontgomeryCountyDataService = _MontgomeryCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
            addressList = await MontgomeryCountyDataService.ReadTopAmountWhereIsGroundRentNull(amountToScrape);
        }
        if (addressList.Count == 0)
        {
            remoteWebDriver.Quit();
            ReportTotals(remoteWebDriver);
        }
        else
        {
            Console.WriteLine("Fresh list read. Restarting scrape.");
            ReportTotals(remoteWebDriver);
            await Scrape(remoteWebDriver, webDriverWait, addressList, amountToScrape);
        }
    }
    private void ReportTotals(RemoteWebDriver remoteWebDriver)
    {
        percentComplete = decimal.Divide(currentCount, totalCount);
        Console.WriteLine($"{remoteWebDriver} has processed {percentComplete:P0} of {totalCount} addresses.");
    }
}

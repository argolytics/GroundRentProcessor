using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DataLibrary.Services.SDATScrapers;

public class CecilCountyScraper : IRealPropertySearchScraper
{
    private readonly IDataContext _dataContext;
    private readonly CecilCountyDataServiceFactory _cecilCountyDataServiceFactory;
    WebDriverModel WebDriverModel { get; set; }
    private string BaseUrl { get; set; } = "https://sdat.dat.maryland.gov/RealProperty/Pages/default.aspx";
    private int AmountToScrape;
    private int currentCount;
    private int totalCount;
    private decimal percentComplete;
    private int exceptionCount = 0;
    private List<AddressModel> AddressList = new();

    public CecilCountyScraper(
        IDataContext dataContext,
        CecilCountyDataServiceFactory cecilCountyDataServiceFactory)
    {
        _dataContext = dataContext;
        _cecilCountyDataServiceFactory = cecilCountyDataServiceFactory;
    }
    public void AllocateWebDrivers(WebDriverModel webDriverModel, int amountToScrape)
    {
        WebDriverModel = webDriverModel;
        AmountToScrape = amountToScrape;
        List<Task> tasks = new()
        {
            Task.Run(() => Scrape())
        };
        Task.WaitAll(tasks.ToArray());
    }
    public async Task Scrape()
    {
        using (var uow = _dataContext.CreateUnitOfWork())
        {
            var CecilCountyDataService = _cecilCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
            WebDriverModel.AddressList = await CecilCountyDataService.ReadTopAmountWhereIsGroundRentNull(AmountToScrape);
        }
        currentCount = 0;
        totalCount = WebDriverModel.AddressList.Count;
        bool result;
        bool checkingIfAddressExists = true;

        WebDriverWait webDriverWait = new(WebDriverModel.Driver, TimeSpan.FromSeconds(10));
        webDriverWait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException),
            typeof(ElementNotSelectableException),
            typeof(ElementNotVisibleException));

        try
        {
            var iterList = WebDriverModel.AddressList.ToList();
            foreach (var address in iterList)
            {
                // Selecting "CECIL COUNTY"
                WebDriverModel.Driver.Navigate().GoToUrl(BaseUrl);
                WebDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlCounty > option:nth-child(9)")));
                WebDriverModel.Input.Click();

                // Selecting "PROPERTY ACCOUNT IDENTIFIER"
                WebDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlSearchType > option:nth-child(3)")));
                WebDriverModel.Input.Click();

                // Click Continue button
                WebDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StartNavigationTemplateContainerID_btnContinue")));
                WebDriverModel.Input.Click();

                // Input Ward
                WebDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtDistrict")));
                WebDriverModel.Input.Clear();
                WebDriverModel.Input.SendKeys(address.Ward);

                // Input AccountNumber
                WebDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtAccountIdentifier")));
                WebDriverModel.Input.Clear();
                WebDriverModel.Input.SendKeys(address.AccountNumber);

                // Click Next button
                WebDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StepNavigationTemplateContainerID_btnStepNextButton")));
                WebDriverModel.Input.Click();
                if (WebDriverModel.Driver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr")).Count != 0)
                {
                    if (WebDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr"))
                        .Text.Contains("There are no records that match your criteria"))
                    {
                        // Address does not exist in SDAT
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var CecilCountyDataService = _cecilCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                            result = await CecilCountyDataService.Delete(address.AccountId);
                        }
                        currentCount++;
                        Console.WriteLine($"{address.AccountId.Trim()} does not exist and was deleted.");
                    }
                    else
                    {
                        Console.WriteLine($"{WebDriverModel.Driver} found {address.AccountId.Trim()} does not exist and tried to delete, but the error message text is different than usual: {WebDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
                        WebDriverModel.Driver.Quit();
                    }
                }
                else
                {
                    // Click Ground Rent Registration link
                    WebDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucDetailsSearch_dlstDetaisSearch_lnkGroundRentRegistration_0")));
                    WebDriverModel.Input.Click();

                    // Condition: check if html has ground rent error tag (meaning property has no ground rent registered)
                    if (WebDriverModel.Driver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Count != 0)
                    {
                        if (WebDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr"))
                            .Text.Contains("There is currently no ground rent"))
                        {
                            // Property is not ground rent
                            address.IsGroundRent = false;
                            using (var uow = _dataContext.CreateUnitOfWork())
                            {
                                var CecilCountyDataService = _cecilCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                                result = await CecilCountyDataService.CreateOrUpdateSDATScraper(new AddressModel
                                {
                                    AccountId = address.AccountId,
                                    IsGroundRent = address.IsGroundRent
                                });
                                currentCount++;
                                WebDriverModel.AddressList.Remove(address);
                            }
                            if (result is false)
                            {
                                // Something wrong happened and I do not want the application to skip over this address
                                WebDriverModel.Driver.Quit();
                                Console.WriteLine($"Db could not complete transaction for {address.AccountId.Trim()}. Quitting scrape.");
                            }
                        }
                        else
                        {
                            WebDriverModel.Driver.Quit();
                            Console.WriteLine($"{WebDriverModel.Driver} found {address.AccountId.Trim()} has a different error message than 'There is currently no ground rent' which is: {WebDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
                        }
                    }
                    else
                    {
                        // Property must be ground rent
                        address.IsGroundRent = true;
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var CecilCountyDataService = _cecilCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                            result = await CecilCountyDataService.CreateOrUpdateSDATScraper(new AddressModel
                            {
                                AccountId = address.AccountId,
                                IsGroundRent = address.IsGroundRent
                            });
                            currentCount++;
                            WebDriverModel.AddressList.Remove(address);
                        }
                        if (result is false)
                        {
                            // Something wrong happened and I do not want the application to skip over this address
                            WebDriverModel.Driver.Quit();
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
                WebDriverModel.Driver.Quit();
                Console.WriteLine("Scraper quit. Exception count passed safety threshold.");
            }
            Console.WriteLine(ex.Message);
            exceptionCount++;
            await RestartScrape();
        }
        finally
        {
            await RestartScrape();
        }
    }
    private async Task RestartScrape()
    {
        WebDriverModel.AddressList.Clear();
        using (var uow = _dataContext.CreateUnitOfWork())
        {
            var CecilCountyDataService = _cecilCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
            AddressList = await CecilCountyDataService.ReadTopAmountWhereIsGroundRentNull(AmountToScrape);
        }
        if (AddressList.Count == 0)
        {
            WebDriverModel.Driver.Quit();
            ReportTotals();
        }
        else
        {
            Console.WriteLine("Fresh list read. Restarting scrape.");
            ReportTotals();
            AllocateWebDrivers(WebDriverModel, AmountToScrape);
        }
    }
    public void StopScraper()
    {
        WebDriverModel.Driver.Quit();
    }
    private void ReportTotals()
    {
        percentComplete = decimal.Divide(currentCount, totalCount);
        Console.WriteLine($"{WebDriverModel.Driver} has processed {percentComplete:P0} of {totalCount} addresses.");
    }
}

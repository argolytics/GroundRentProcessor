using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DataLibrary.Services.SDATScrapers;

public class BaltimoreCountyScraper : IRealPropertySearchScraper
{
    private readonly IDataContext _dataContext;
    private readonly IGroundRentProcessorDataServiceFactory _groundRentProcessorDataServiceFactory;
    private WebDriver FirefoxDriver { get; set; } = null;
    private IWebElement FirefoxInput { get; set; }
    private string FirefoxDriverPath { get; set; } = @"C:\WebDrivers\geckodriver.exe";
    private string BaseUrl { get; set; } = "https://sdat.dat.maryland.gov/RealProperty/Pages/default.aspx";

    public BaltimoreCountyScraper(
        IDataContext dataContext,
        IGroundRentProcessorDataServiceFactory groundRentProcessorDataServiceFactory)
    {
        _dataContext = dataContext;
        _groundRentProcessorDataServiceFactory = groundRentProcessorDataServiceFactory;

        // For Amanda's laptop
        //FirefoxProfile firefoxProfile = new(@"C:\Users\Jason Argo\AppData\Local\Mozilla\Firefox\Profiles\4x0ows5f.default-release-1670017618762");
        //FirefoxOptions firefoxOptions = new();
        //firefoxOptions.Profile = firefoxProfile;
        //firefoxOptions.AddArguments("--headless");
        //firefoxOptions.AddArguments("--binary C:\\Program Files\\Mozilla Firefox\\firefox.exe");
        //FirefoxDriver = new FirefoxDriver(FirefoxDriverPath, firefoxOptions, TimeSpan.FromSeconds(30));

        FirefoxProfile firefoxProfile = new(@"C:\WebDrivers\FirefoxProfile-DetaultUser");
        FirefoxOptions firefoxOptions = new();
        firefoxOptions.Profile = firefoxProfile;
        firefoxOptions.AddArguments("--headless");
        FirefoxDriver = new FirefoxDriver(FirefoxDriverPath, firefoxOptions, TimeSpan.FromSeconds(30));
    }
    public void AllocateWebDrivers(
        List<AddressModel> firefoxAddressList, int amountToScrape)
    {
        WebDriverModel firefoxDriverModel = new WebDriverModel
        {
            Driver = FirefoxDriver,
            Input = FirefoxInput,
            AddressList = firefoxAddressList
        };

        List<Task> tasks = new();
        tasks.Add(Task.Run(() => Scrape(firefoxDriverModel)));
        Task.WaitAll(tasks.ToArray());

    }
    public async Task Scrape(WebDriverModel webDriverModel)
    {
        int currentCount;
        var totalCount = webDriverModel.AddressList.Count;
        bool result;
        bool checkingIfAddressExists = true;

        WebDriverWait webDriverWait = new(webDriverModel.Driver, TimeSpan.FromSeconds(10));
        webDriverWait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException),
            typeof(ElementNotSelectableException),
            typeof(ElementNotVisibleException));

        try
        {
            Console.WriteLine($"{webDriverModel.Driver} begin...");
            foreach (var address in webDriverModel.AddressList)
            {
                currentCount = webDriverModel.AddressList.IndexOf(address) + 1;
                // Selecting "BALTIMORE COUNTY"
                webDriverModel.Driver.Navigate().GoToUrl(BaseUrl);
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlCounty > option:nth-child(5)")));
                webDriverModel.Input.Click();

                // Selecting "PROPERTY ACCOUNT IDENTIFIER"
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlSearchType > option:nth-child(3)")));
                webDriverModel.Input.Click();

                // Click Continue button
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StartNavigationTemplateContainerID_btnContinue")));
                webDriverModel.Input.Click();

                // Input Ward
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtDistrict")));
                webDriverModel.Input.Clear();
                webDriverModel.Input.SendKeys(address.Ward);

                // Input AccountNumber
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtAccountIdentifier")));
                webDriverModel.Input.Clear();
                webDriverModel.Input.SendKeys(address.AccountNumber);

                // Click Next button
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StepNavigationTemplateContainerID_btnStepNextButton")));
                webDriverModel.Input.Click();
                if (webDriverModel.Driver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr")).Count != 0)
                {
                    if (webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr"))
                        .Text.Contains("There are no records that match your criteria"))
                    {
                        // Address does not exist in SDAT
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var groundRentProcessorDataService = _groundRentProcessorDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                            result = await groundRentProcessorDataService.Delete(address.AccountId);
                            Console.WriteLine($"{address.AccountId.Trim()} does not exist and was deleted.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{webDriverModel.Driver} found {address.AccountId.Trim()} does not exist and tried to delete, but the error message text is different than usual: {webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
                        webDriverModel.Driver.Quit();
                    }
                }
                else
                {
                    // Click Ground Rent Registration link
                    webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucDetailsSearch_dlstDetaisSearch_lnkGroundRentRegistration_0")));
                    webDriverModel.Input.Click();

                    // Condition: check if html has ground rent error tag (meaning property has no ground rent registered)

                    if (webDriverModel.Driver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Count != 0)
                    {
                        if (webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr"))
                            .Text.Contains("There is currently no ground rent"))
                        {
                            // Property is not ground rent
                            address.IsGroundRent = false;
                            using (var uow = _dataContext.CreateUnitOfWork())
                            {
                                var groundRentProcessorDataService = _groundRentProcessorDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                                result = await groundRentProcessorDataService.CreateOrUpdateSDATScraper(new AddressModel
                                {
                                    AccountId = address.AccountId,
                                    IsGroundRent = address.IsGroundRent
                                });
                                Console.WriteLine($"{address.AccountId.Trim()} is fee simple.");
                            }
                            if (result is false)
                            {
                                // Something wrong happened and I do not want the application to skip over this address
                                webDriverModel.Driver.Quit();
                                Console.WriteLine($"Db could not complete transaction for {address.AccountId.Trim()}. Call Jason.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{webDriverModel.Driver} found {address.AccountId.Trim()} has a different error message than 'There is currently no ground rent' which is: {webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
                            webDriverModel.Driver.Quit();
                        }
                    }
                    else
                    {
                        // Property must be ground rent
                        address.IsGroundRent = true;
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var groundRentProcessorDataService = _groundRentProcessorDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                            result = await groundRentProcessorDataService.CreateOrUpdateSDATScraper(new AddressModel
                            {
                                AccountId = address.AccountId,
                                IsGroundRent = address.IsGroundRent
                            });
                            Console.WriteLine($"{address.AccountId.Trim()} is ground rent.");
                        }
                        if (result is false)
                        {
                            // Something wrong happened and I do not want the application to skip over this address
                            webDriverModel.Driver.Quit();
                            Console.WriteLine($"Db could not complete transaction for {address.AccountId.Trim()}. Call Jason.");
                        }
                    }
                    decimal percentComplete = decimal.Divide(currentCount, totalCount);
                    Console.WriteLine($"{webDriverModel.Driver} has processed {percentComplete:P0} of addresses in list.");
                }
            }
        }
        catch (NoSuchElementException ex)
        {
            Console.WriteLine($"{webDriverModel.Driver} ran into the following exception: {ex.Message}");
            Thread.Sleep(3000);
        }
        catch (StaleElementReferenceException ex)
        {
            Console.WriteLine($"{webDriverModel.Driver} ran into the following exception: {ex.Message}");
            Thread.Sleep(3000);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"{webDriverModel.Driver} ran into the following exception: {ex.Message}");
            Thread.Sleep(3000);
        }
        catch (WebDriverException ex)
        {
            Console.WriteLine($"{webDriverModel.Driver} ran into the following exception: {ex.Message}");
            Thread.Sleep(3000);
        }
        webDriverModel.Driver.Quit();
    }
}

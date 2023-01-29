using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DataLibrary.Services.SDATScrapers;

public class BaltimoreCityScraper : IRealPropertySearchScraper
{
    private readonly IDataContext _dataContext;
    private readonly BaltimoreCityDataServiceFactory _baltimoreCityDataServiceFactory;
    FirefoxDriver FirefoxDriver;
    private readonly string FirefoxDriverPath = @"C:\WebDrivers\geckodriver.exe";
    private readonly string FirefoxProfile = @"C:\Users\Jason\AppData\Local\Mozilla\Firefox\Profiles\7mdph1dj.AspxConverter";
    WebDriverWait WebDriverWait;
    private IWebElement Input { get; set; }
    private List<AddressModel> AddressList = new();
    private string BaseUrl { get; set; } = "https://sdat.dat.maryland.gov/RealProperty/Pages/default.aspx";
    private bool? dbTransactionResult = null;
    private bool? pdfDownloaded = null;
    private int currentCount;
    private int totalCount;
    private decimal percentComplete;
    private int exceptionCount = 0;

    public BaltimoreCityScraper(
        IDataContext dataContext,
        BaltimoreCityDataServiceFactory baltimoreCityDataServiceFactory)
    {
        _dataContext = dataContext;
        _baltimoreCityDataServiceFactory = baltimoreCityDataServiceFactory;

        FirefoxProfile firefoxProfile = new(FirefoxProfile);
        FirefoxOptions FirefoxOptions = new()
        {
            Profile = firefoxProfile,
        };
        //firefoxOptions.AddArguments("--headless");
        FirefoxDriver = new FirefoxDriver(FirefoxDriverPath, FirefoxOptions, TimeSpan.FromSeconds(20));
        WebDriverWait = new(FirefoxDriver, TimeSpan.FromSeconds(20));
        WebDriverWait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException),
            typeof(ElementNotSelectableException),
            typeof(ElementNotVisibleException));
    }
    public async Task Scrape(int amountToScrape)
    {
        using (var uow = _dataContext.CreateUnitOfWork())
        {
            var baltimoreCityDataService = _baltimoreCityDataServiceFactory.CreateGroundRentProcessorDataService(uow);
            //AddressList = await baltimoreCityDataService.ReadTopAmountWhereIsGroundRentNull(amountToScrape);
            AddressList = await baltimoreCityDataService.ReadTopAmountWhereIsGroundRentTrue(amountToScrape);
        }
        currentCount = 0;
        totalCount = AddressList.Count;

        try
        {
            var iterList = AddressList.ToList();
            foreach (var address in iterList)
            {
                // Selecting "BALTIMORE CITY"
                FirefoxDriver.Navigate().GoToUrl(BaseUrl);
                Input = WebDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlCounty > option:nth-child(4)")));
                Input.Click();

                // Selecting "PROPERTY ACCOUNT IDENTIFIER"
                Input = WebDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlSearchType > option:nth-child(3)")));
                Input.Click();

                // Click Continue button
                Input = WebDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StartNavigationTemplateContainerID_btnContinue")));
                Input.Click();

                // Input Ward
                Input = WebDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtWard")));
                Input.Clear();
                Input.SendKeys(address.Ward);

                // Input Section
                Input = WebDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtSection")));
                Input.Clear();
                Input.SendKeys(address.Section);

                // Input Block
                Input = WebDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtBlock")));
                Input.Clear();
                Input.SendKeys(address.Block);

                // Input Lot
                Input = WebDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtLot")));
                Input.Clear();
                Input.SendKeys(address.Lot);

                // Click Next button
                Input = WebDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StepNavigationTemplateContainerID_btnStepNextButton")));
                Input.Click();
                if (FirefoxDriver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr")).Count != 0)
                {
                    if (FirefoxDriver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr"))
                        .Text.Contains("There are no records that match your criteria"))
                    {
                        // Address does not exist in SDAT
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var baltimoreCityDataService = _baltimoreCityDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                            dbTransactionResult = await baltimoreCityDataService.Delete(address.AccountId);
                        }
                        if (dbTransactionResult is false)
                        {
                            Console.WriteLine($"Could not delete {address.AccountId}. Quitting scrape.");
                            FirefoxDriver.Quit();
                        }
                        currentCount++;
                        Console.WriteLine($"{address.AccountId.Trim()} does not exist and was deleted.");
                    }
                    else
                    {
                        Console.WriteLine($"{FirefoxDriver} found {address.AccountId.Trim()} does not exist and tried to delete, but the error message text is different than usual: {FirefoxDriver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
                        FirefoxDriver.Quit();
                    }
                }
                else
                {
                    // Click Ground Rent Registration link
                    WebDriverWait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("head > style:nth-child(29)")));
                    Input = WebDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucDetailsSearch_dlstDetaisSearch_lnkGroundRentRegistration_0")));
                    Input.Click();
                    var firstWindow = FirefoxDriver.CurrentWindowHandle;
                    // Condition: check if html has ground rent error tag (meaning property has no ground rent registered)
                    if (FirefoxDriver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Count != 0)
                    {
                        if (FirefoxDriver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr"))
                            .Text.Contains("There is currently no ground rent"))
                        {
                            // Property is not ground rent
                            address.IsGroundRent = false;
                            using (var uow = _dataContext.CreateUnitOfWork())
                            {
                                var BaltimoreCityDataService = _baltimoreCityDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                                dbTransactionResult = await BaltimoreCityDataService.CreateOrUpdateSDATScraper(new AddressModel
                                {
                                    AccountId = address.AccountId,
                                    IsGroundRent = address.IsGroundRent,
                                    PdfDownloaded = null
                                });
                                currentCount++;
                                AddressList.Remove(address);
                            }
                            if (dbTransactionResult is false)
                            {
                                // Something wrong happened and I do not want the application to skip over this address
                                Console.WriteLine($"Db could not complete transaction for {address.AccountId.Trim()}. Quitting scrape.");
                                FirefoxDriver.Quit();
                            }
                        }
                    }
                    else
                    {
                        // Property must be ground rent
                        address.IsGroundRent = true;
                        // Determine child count of pdf list
                        var pdfLinkArray = FirefoxDriver.FindElements(By.XPath("//table[@id='cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_gv_GRRegistratonResult']/tbody/tr"));
                        var registrationPdfElementId = pdfLinkArray[pdfLinkArray.Count - 2].FindElement(By.TagName("a")).GetAttribute("id");
                        // Grab Ground Rent Registration PDF
                        Input = WebDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.Id(registrationPdfElementId)));
                        Input.Click();
                        foreach (string window in FirefoxDriver.WindowHandles)
                        {
                            if (firstWindow != window)
                            {
                                FirefoxDriver.SwitchTo().Window(window);
                            }
                        }
                        var accountId = address.AccountId.Trim();
                        if (WebDriverWait.Until(FirefoxDriver => ((IJavaScriptExecutor)FirefoxDriver).ExecuteScript("return document.readyState").Equals("complete")))
                        {
                            PrintOptions printOptions = new();
                            var pdf = FirefoxDriver.Print(printOptions);
                            pdf.SaveAsFile($@"C:\Users\Jason\Desktop\GroundRentRegistrationPdfs\BACI\{accountId}.pdf");
                            pdfDownloaded = true;
                        }
                        if (pdfDownloaded is true)
                        {
                            address.PdfDownloaded = true;
                        }
                        else
                        {
                            pdfDownloaded = false;
                        }
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var baltimoreCityDataService = _baltimoreCityDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                            dbTransactionResult = await baltimoreCityDataService.CreateOrUpdateSDATScraper(new AddressModel
                            {
                                AccountId = address.AccountId,
                                IsGroundRent = address.IsGroundRent,
                                PdfDownloaded = address.PdfDownloaded
                            });
                        }
                        if (dbTransactionResult is false)
                        {
                            FirefoxDriver.Quit();
                        }
                        currentCount++;
                        AddressList.Remove(address);
                        FirefoxDriver.Close();
                        FirefoxDriver.SwitchTo().Window(firstWindow);
                    }
                }
            }
            ReportTotals(FirefoxDriver);
            await RestartScrape(amountToScrape);
        }
        catch(ArgumentOutOfRangeException argumentOutOfRangeException)
        {
            exceptionCount++;
            Console.WriteLine($"Total exception count: {exceptionCount}");
            Console.WriteLine($"Error Message: {argumentOutOfRangeException.Message}");
            await RestartScrape(amountToScrape);
        }
        catch(ElementClickInterceptedException elementClickInterceptedException)
        {
            exceptionCount++;
            Console.WriteLine($"Total exception count: {exceptionCount}");
            Console.WriteLine($"Error Message: {elementClickInterceptedException.Message}");
            await RestartScrape(amountToScrape);
        }
        catch(WebDriverTimeoutException webDriverTimeoutException)
        {
            exceptionCount++;
            Console.WriteLine($"Total exception count: {exceptionCount}");
            Console.WriteLine($"Error Message: {webDriverTimeoutException.Message}");
            await RestartScrape(amountToScrape);
        }
        catch (NullReferenceException nullReferenceException)
        {
            exceptionCount++;
            Console.WriteLine($"Total exception count: {exceptionCount}");
            Console.WriteLine($"Error Message: {nullReferenceException.Message}");
            await RestartScrape(amountToScrape);
        }
        catch(NoSuchWindowException noSuchWindowException)
        {
            exceptionCount++;
            Console.WriteLine($"Total exception count: {exceptionCount}");
            Console.WriteLine($"Error Message: {noSuchWindowException.Message}");
            FirefoxProfile firefoxProfile = new(FirefoxProfile);
            FirefoxOptions FirefoxOptions = new()
            {
                Profile = firefoxProfile,
            };
            FirefoxDriver = new FirefoxDriver(FirefoxDriverPath, FirefoxOptions, TimeSpan.FromSeconds(20));
            WebDriverWait = new(FirefoxDriver, TimeSpan.FromSeconds(20));
            WebDriverWait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(StaleElementReferenceException),
                typeof(ElementNotSelectableException),
                typeof(ElementNotVisibleException));
            await RestartScrape(amountToScrape);
        }
        catch (WebDriverException webDriverException)
        {
            exceptionCount++;
            Console.WriteLine($"Total exception count: {exceptionCount}");
            Console.WriteLine($"Error Message: {webDriverException.Message}");
            await RestartScrape(amountToScrape);
        }
    }
    private async Task RestartScrape(int amountToScrape)
    {
        AddressList.Clear();
        using (var uow = _dataContext.CreateUnitOfWork())
        {
            var baltimoreCityDataService = _baltimoreCityDataServiceFactory.CreateGroundRentProcessorDataService(uow);
            AddressList = await baltimoreCityDataService.ReadTopAmountWhereIsGroundRentTrue(amountToScrape);
        }
        if (AddressList.Count == 0)
        {
            FirefoxDriver.Quit();
            Console.WriteLine("Baltimore City complete.");
            ReportTotals(FirefoxDriver);
        }
        else
        {
            await Scrape(amountToScrape);
        }
    }
    private void ReportTotals(FirefoxDriver FirefoxDriver)
    {
        percentComplete = totalCount == 0 ? 0 : decimal.Divide(currentCount, totalCount);
        Console.WriteLine($"{FirefoxDriver} has processed {percentComplete:P0} of {totalCount} addresses.");
    }
}

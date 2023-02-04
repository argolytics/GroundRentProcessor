using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
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
    private int currentCount;
    private int totalCount;
    private decimal percentComplete;
    private int exceptionCount = 0;
    private int firefoxWindowHandleCount = 0;
    private int pdfTotalCount = 0;
    private int pdfDownloadCount = 0;

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
        // Initialize base window
        var baseUrlWindow = FirefoxDriver.CurrentWindowHandle;
        // Close out all other windows other than base window if they exist
        foreach (string window in FirefoxDriver.WindowHandles)
        {
            if (baseUrlWindow != window)
            {
                FirefoxDriver.Close();
            }
        }
        FirefoxDriver.SwitchTo().Window(baseUrlWindow);
        // Read and populate address list
        using (var uow = _dataContext.CreateUnitOfWork())
        {
            var baltimoreCityDataService = _baltimoreCityDataServiceFactory.CreateGroundRentProcessorDataService(uow);
            AddressList = await baltimoreCityDataService.ReadTopAmountWhereIsGroundRentNull(amountToScrape);
            //AddressList = await baltimoreCityDataService.ReadTopAmountWhereIsGroundRentTrue(amountToScrape);
        }
        if (AddressList.Count == 0)
        {
            foreach (string window in FirefoxDriver.WindowHandles)
            {
                FirefoxDriver.Close();
            }
            FirefoxDriver.Quit();
            Console.WriteLine("Baltimore City complete.");
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
                                    AllPdfsDownloaded = null
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
                        // Fully load XPath table
                        const string tableXPath = @"//table[@id='cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_gv_GRRegistratonResult']";
                        WebDriverWait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath(tableXPath)));
                        // Determine PDF link element count
                        var pdfLinkList = FirefoxDriver.FindElements(By.XPath($"{tableXPath}/tbody/tr")).ToList();
                        pdfLinkList.Remove(pdfLinkList[0]); // removing header row
                        pdfLinkList.Remove(pdfLinkList.Last()); // removing empty last row
                        var pdfTotalCount = pdfLinkList.Count;
                        // Initialize various PDF metadata within scope (and account id)
                        List<GroundRentPdfModel> groundRentPdfModelList = new();
                        var accountId = address.AccountId.Trim();
                        // Grab pdf metadata: Date and Time, Document Type, Acknowledgement Number, PDF Page Count, and Deed Reference info
                        foreach (var pdfLink in pdfLinkList)
                        {
                            GroundRentPdfModel groundRentPdfModel = new();
                            groundRentPdfModelList.Add(groundRentPdfModel);
                            groundRentPdfModel.AccountId = accountId;
                            var dateTimeFiled = pdfLink.FindElement(By.XPath("//span[contains(@id, 'txtDateFiled')]")).Text;
                            groundRentPdfModel.DocumentFiledType = pdfLink.FindElement(By.XPath("//span[contains(@id, 'txtDocument')]")).Text;
                            groundRentPdfModel.AcknowledgementNumber = pdfLink.FindElement(By.XPath("//span[contains(@id, 'txtAcknowledgement')]")).Text;
                            groundRentPdfModel.PdfPageCount = pdfLink.FindElement(By.XPath("//span[contains(@id, 'txtpages')]")).Text;
                            var deedReferenceData = pdfLink.FindElement(By.XPath("//span[contains(@id, 'txtDeedRef')]")).Text;
                            bool? dateTimeConverted = null;
                            DateTime dateTimeResult = new();
                            if (DateTime.TryParseExact(dateTimeFiled, "MM/dd/YYYY hh:mm:ss tt", null,
                                System.Globalization.DateTimeStyles.None, out dateTimeResult))
                            {
                                dateTimeConverted = true;
                                dateTimeFiled = string.Empty;
                            }
                            var deedReferenceDataArray = deedReferenceData.Split('/');
                            groundRentPdfModel.Book = deedReferenceDataArray[0];
                            groundRentPdfModel.Page = deedReferenceDataArray[1];
                            groundRentPdfModel.ClerkInitials = deedReferenceDataArray[2];
                            int.TryParse(deedReferenceDataArray[3], out var yearRecordedResult);
                            groundRentPdfModel.YearRecorded = yearRecordedResult;
                            using (var uow = _dataContext.CreateUnitOfWork())
                            {
                                var baltimoreCityDataService = _baltimoreCityDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                                dbTransactionResult = await baltimoreCityDataService.CreateOrUpdateGroundRentPdf(groundRentPdfModel);
                            }
                            if (dbTransactionResult is false)
                            {
                                foreach (string window in FirefoxDriver.WindowHandles)
                                {
                                    FirefoxDriver.Close();
                                }
                                FirefoxDriver.Quit();
                            }
                            // Select and click on PDF link
                            Input = WebDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.Id(pdfLink.FindElement(By.TagName("a")).GetAttribute("id"))));
                            Input.Click();
                        }
                        // Switch to PDF link window
                        foreach (string window in FirefoxDriver.WindowHandles)
                        {
                            var groundRentPdfModelListCurrentCount = 0;
                            var groundRentPdfModelListTotalCount = groundRentPdfModelList.Count;
                            if (baseUrlWindow != window)
                            {
                                FirefoxDriver.SwitchTo().Window(window);
                                // Download PDF
                                if (WebDriverWait.Until(FirefoxDriver => ((IJavaScriptExecutor)FirefoxDriver).ExecuteScript("return document.readyState").Equals("complete")))
                                {
                                    PrintOptions printOptions = new();
                                    FirefoxDriver.Print(printOptions).SaveAsFile($@"C:\Users\Jason\Desktop\GroundRentRegistrationPdfs\BACI\{accountId}_{groundRentPdfModelList[groundRentPdfModelListCurrentCount].DocumentFiledType}{groundRentPdfModelList[groundRentPdfModelListCurrentCount].AcknowledgementNumber}.pdf");
                                    pdfDownloadCount++;
                                }
                                groundRentPdfModelListCurrentCount++;
                                FirefoxDriver.Close();
                            }
                        }
                        FirefoxDriver.SwitchTo().Window(baseUrlWindow);
                        if (pdfDownloadCount == pdfTotalCount)
                        {
                            address.PdfCount = pdfTotalCount;
                            address.AllPdfsDownloaded = true;
                            
                            using (var uow = _dataContext.CreateUnitOfWork())
                            {
                                var baltimoreCityDataService = _baltimoreCityDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                                dbTransactionResult = await baltimoreCityDataService.CreateOrUpdateSDATScraper(new AddressModel
                                {
                                    AccountId = address.AccountId,
                                    IsGroundRent = address.IsGroundRent,
                                    PdfCount = address.PdfCount,
                                    AllPdfsDownloaded = address.AllPdfsDownloaded
                                });
                            }
                            if (dbTransactionResult is false)
                            {
                                foreach (string window in FirefoxDriver.WindowHandles)
                                {
                                    FirefoxDriver.Close();
                                }
                                FirefoxDriver.Quit();
                            }
                        }
                        else
                        {
                            address.PdfCount = null;
                            address.AllPdfsDownloaded = false;
                            using (var uow = _dataContext.CreateUnitOfWork())
                            {
                                var baltimoreCityDataService = _baltimoreCityDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                                dbTransactionResult = await baltimoreCityDataService.CreateOrUpdateSDATScraper(new AddressModel
                                {
                                    AccountId = address.AccountId,
                                    IsGroundRent = address.IsGroundRent,
                                    PdfCount = address.PdfCount,
                                    AllPdfsDownloaded = address.AllPdfsDownloaded
                                });
                            }
                            if (dbTransactionResult is false)
                            {
                                foreach (string window in FirefoxDriver.WindowHandles)
                                {
                                    FirefoxDriver.Close();
                                }
                                FirefoxDriver.Quit();
                            }
                        }
                        currentCount++;
                        AddressList.Remove(address);
                        FirefoxDriver.Close();
                        FirefoxDriver.SwitchTo().Window(baseUrlWindow);
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
        await Scrape(amountToScrape);
    }
    private void ReportTotals(FirefoxDriver FirefoxDriver)
    {
        percentComplete = totalCount == 0 ? 0 : decimal.Divide(currentCount, totalCount);
        Console.WriteLine($"{FirefoxDriver} has processed {percentComplete:P0} of {totalCount} addresses.");
    }
}

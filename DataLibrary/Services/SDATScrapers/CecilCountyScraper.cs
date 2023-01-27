using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DataLibrary.Services.SDATScrapers;

public class CecilCountyScraper : IRealPropertySearchScraper
{
    private readonly IDataContext _dataContext;
    private readonly CecilCountyDataServiceFactory _cecilCountyDataServiceFactory;
    private IWebElement Input { get; set; }
    private List<AddressModel> AddressList = new();
    private string BaseUrl { get; set; } = "https://sdat.dat.maryland.gov/RealProperty/Pages/default.aspx";
    private bool? dbTransactionResult = null;
    private bool? pdfDownloaded = null;
    private int currentCount;
    private int totalCount;
    private decimal percentComplete;
    private int exceptionCount = 0;

    public CecilCountyScraper(
        IDataContext dataContext,
        CecilCountyDataServiceFactory cecilCountyDataServiceFactory)
    {
        _dataContext = dataContext;
        _cecilCountyDataServiceFactory = cecilCountyDataServiceFactory;
    }
    public async Task Scrape(
        FirefoxDriver firefoxDriver,
        WebDriverWait webDriverWait,
        int amountToScrape)
    {
        using (var uow = _dataContext.CreateUnitOfWork())
        {
            var cecilCountyDataService = _cecilCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
            //AddressList = await cecilCountyDataService.ReadTopAmountWhereIsGroundRentNull(amountToScrape);
            AddressList = await cecilCountyDataService.ReadTopAmountWhereIsGroundRentNull(amountToScrape);
        }
        currentCount = 0;
        totalCount = AddressList.Count;

        try
        {
            var iterList = AddressList.ToList();
            foreach (var address in iterList)
            {
                // Selecting "CECIL COUNTY"
                firefoxDriver.Navigate().GoToUrl(BaseUrl);
                Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlCounty > option:nth-child(9)")));
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
                //Input.SendKeys("01");

                // Input AccountNumber
                Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtAccountIdentifier")));
                Input.Clear();
                Input.SendKeys(address.AccountNumber);
                //Input.SendKeys("0101130680");

                // Click Next button
                Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StepNavigationTemplateContainerID_btnStepNextButton")));
                Input.Click();
                if (firefoxDriver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr")).Count != 0)
                {
                    if (firefoxDriver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr"))
                        .Text.Contains("There are no records that match your criteria"))
                    {
                        // Address does not exist in SDAT
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var cecilCountyDataService = _cecilCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                            dbTransactionResult = await cecilCountyDataService.Delete(address.AccountId);
                        }
                        if (dbTransactionResult is false)
                        {
                            Console.WriteLine($"Could not delete {address.AccountId}. Quitting scrape.");
                            firefoxDriver.Quit();
                        }
                        currentCount++;
                        Console.WriteLine($"{address.AccountId.Trim()} does not exist and was deleted.");
                    }
                    else
                    {
                        Console.WriteLine($"{firefoxDriver} found {address.AccountId.Trim()} does not exist and tried to delete, but the error message text is different than usual: {firefoxDriver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
                        firefoxDriver.Quit();
                    }
                }
                else
                {
                    // Click Ground Rent Registration link
                    webDriverWait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("head > style:nth-child(29)")));
                    Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucDetailsSearch_dlstDetaisSearch_lnkGroundRentRegistration_0")));
                    Input.Click();
                    var firstWindow = firefoxDriver.CurrentWindowHandle;
                    // Condition: check if html has ground rent error tag (meaning property has no ground rent registered)
                    if (firefoxDriver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Count != 0)
                    {
                        if (firefoxDriver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr"))
                            .Text.Contains("There is currently no ground rent"))
                        {
                            // Property is not ground rent
                            address.IsGroundRent = false;
                            using (var uow = _dataContext.CreateUnitOfWork())
                            {
                                var CecilCountyDataService = _cecilCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                                dbTransactionResult = await CecilCountyDataService.CreateOrUpdateSDATScraper(new AddressModel
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
                                firefoxDriver.Quit();
                            }
                        }
                    }
                    else
                    {
                        // Property must be ground rent
                        address.IsGroundRent = true;
                        // Determine child count of pdf list
                        var pdfTableRowCount = firefoxDriver.FindElements(By.XPath("//table[@id='cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_gv_GRRegistratonResult']/tbody/tr")).Count;
                        // Grab Ground Rent Registration PDF
                        Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector($"#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_gv_GRRegistratonResult_lnkMakePDF_{pdfTableRowCount - 3}")));
                        Input.Click();
                        foreach (string window in firefoxDriver.WindowHandles)
                        {
                            if (firstWindow != window)
                            {
                                firefoxDriver.SwitchTo().Window(window);
                            }
                        }
                        var accountId = address.AccountId.Trim();
                        if (webDriverWait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete")))
                        {
                            PrintOptions printOptions = new();
                            var pdf = firefoxDriver.Print(printOptions);
                            pdf.SaveAsFile($@"C:\Users\Jason\Desktop\GroundRentRegistrationPdfs\CECI\{accountId}.pdf");
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
                            var cecilCountyDataService = _cecilCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                            dbTransactionResult = await cecilCountyDataService.CreateOrUpdateSDATScraper(new AddressModel
                            {
                                AccountId = address.AccountId,
                                IsGroundRent = address.IsGroundRent,
                                PdfDownloaded = address.PdfDownloaded
                            });
                        }
                        if (dbTransactionResult is false)
                        {
                            firefoxDriver.Quit();
                        }
                        currentCount++;
                        AddressList.Remove(address);
                        firefoxDriver.Close();
                        firefoxDriver.SwitchTo().Window(firstWindow);
                    }
                }
            }
            ReportTotals(firefoxDriver);
        }
        catch (Exception ex)
        {
            if (exceptionCount > 3)
            {
                firefoxDriver.Quit();
                Console.WriteLine("Scraper quit. Exception count passed safety threshold.");
            }
            Console.WriteLine(ex.Message);
            exceptionCount++;
            firefoxDriver.Quit();
            ReportTotals(firefoxDriver);
            //await RestartScrape(firefoxDriver, webDriverWait, addressList, amountToScrape);
        }
        finally
        {
            firefoxDriver.Quit();
            //await RestartScrape(firefoxDriver, webDriverWait, addressList, amountToScrape);
        }
    }

    private async Task RestartScrape(
        FirefoxDriver firefoxDriver,
        WebDriverWait webDriverWait,
        int amountToScrape)
    {
        AddressList.Clear();
        using (var uow = _dataContext.CreateUnitOfWork())
        {
            var cecilCountyDataService = _cecilCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
            AddressList = await cecilCountyDataService.ReadTopAmountWhereIsGroundRentNull(amountToScrape);
        }
        if (AddressList.Count == 0)
        {
            firefoxDriver.Quit();
            ReportTotals(firefoxDriver);
        }
        else
        {
            Console.WriteLine("Fresh list read. Restarting scrape.");
            ReportTotals(firefoxDriver);
            await Scrape(firefoxDriver, webDriverWait, amountToScrape);
        }
    }
    private void ReportTotals(FirefoxDriver firefoxDriver)
    {
        percentComplete = decimal.Divide(currentCount, totalCount);
        Console.WriteLine($"{firefoxDriver} has processed {percentComplete:P0} of {totalCount} addresses.");
    }
}

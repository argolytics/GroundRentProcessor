using DataLibrary.Models;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace DataLibrary.Services.SDATScrapers;

public interface IRealPropertySearchScraper
{
    Task Scrape(
        FirefoxDriver FirefoxWebDriver, 
        WebDriverWait WebDriverWait, 
        int AmountToScrape);
}
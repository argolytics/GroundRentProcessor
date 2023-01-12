using DataLibrary.Models;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace DataLibrary.Services.SDATScrapers;

public interface IRealPropertySearchScraper
{
    Task Scrape(
        RemoteWebDriver RemoteWebDriver, 
        WebDriverWait WebDriverWait, 
        List<AddressModel> AddressModel, 
        int AmountToScrape);
}
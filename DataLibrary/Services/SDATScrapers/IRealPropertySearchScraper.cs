using DataLibrary.Models;

namespace DataLibrary.Services.SDATScrapers;

public interface IRealPropertySearchScraper
{
    void AllocateWebDrivers(WebDriverModel webDriverModel, int amountToScrape);
    Task Scrape();
}
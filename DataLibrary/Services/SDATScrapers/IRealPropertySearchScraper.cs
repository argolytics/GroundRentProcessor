namespace DataLibrary.Services.SDATScrapers;

public interface IRealPropertySearchScraper
{
    Task Scrape(int AmountToScrape);
}
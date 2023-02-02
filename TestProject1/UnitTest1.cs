using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Services.SDATScrapers;
using Microsoft.Extensions.Configuration;

namespace TestProject1;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public async Task TestMethod1() {
        var t = new BaltimoreCityScraper(
            new DataContext("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GroundRentProcessorDb;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"),
            new BaltimoreCityDataServiceFactory());

        await t.Scrape(2);

    }
}
using DataLibrary.Models;

namespace DataLibrary.DbServices
{
    public interface IGroundRentProcessorDataService
    {
        Task CreateOrUpdateFile(AddressModel addressModel);
        Task CreateOrUpdateSDATRedeemedFile(AddressModel addressModel);
        Task<bool> CreateOrUpdateSDATScraper(AddressModel addressModel);
        Task<bool> Delete(string accountId);
        Task<List<AddressModel>> ReadTopAmountWhereIsGroundRentNull(int amount);
        Task<List<AddressModel>> ReadTopAmountWhereIsGroundRentTrue(int amount);
    }
}
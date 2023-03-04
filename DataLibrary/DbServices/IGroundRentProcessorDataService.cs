using DataLibrary.Models;

namespace DataLibrary.DbServices
{
    public interface IGroundRentProcessorDataService
    {
        Task CreateOrUpdateFile(AddressModel addressModel);
        Task CreateOrUpdateSDATRedeemedFile(AddressModel addressModel);
        Task<bool> CreateAddress(AddressModel addressModel);
        Task<bool> UpdateAddress(AddressModel addressModel);
        Task<bool> CreateGroundRentPdf(GroundRentPdfModel groundRentPdfModel);
        Task<bool> UpdateGroundRentPdf(GroundRentPdfModel groundRentPdfModel);
        Task<bool> DeleteAddress(string accountId);
        Task<List<AddressModel>> ReadAddressTopAmountWhereIsGroundRentNull(int amount);
        Task<List<AddressModel>> ReadAddressTopAmountWhereIsGroundRentTrue(int amount);
    }
}
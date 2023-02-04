using DataLibrary.Models;
using Dapper;
using System.Data;
using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class BaltimoreCitySqlDataService : IGroundRentProcessorDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public BaltimoreCitySqlDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task CreateOrUpdateFile(AddressModel addressModel)
    {
        var parms = new
        {
            addressModel.AccountId,
            addressModel.Ward,
            addressModel.Section,
            addressModel.Block,
            addressModel.Lot,
            addressModel.LandUseCode,
            addressModel.YearBuilt
        };
        await _unitOfWork.Connection.ExecuteAsync("spBaltimoreCity_CreateOrUpdateFile", parms,
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
    }
    public async Task CreateOrUpdateSDATRedeemedFile(AddressModel addressModel)
    {
        return;
    }
    public async Task<bool> CreateOrUpdateSDATScraper(AddressModel addressModel)
    {
        try
        {
            var parms = new
            {
                addressModel.AccountId,
                addressModel.IsGroundRent,
                addressModel.PdfCount,
                addressModel.AllPdfsDownloaded
            };
            await _unitOfWork.Connection.ExecuteAsync("spBaltimoreCity_CreateOrUpdateSDATScraper", parms,
                commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    public async Task<bool> CreateOrUpdateGroundRentPdf(GroundRentPdfModel groundRentPdfModel)
    {
        try
        {
            var parms = new
            {
                groundRentPdfModel.AccountId,
                groundRentPdfModel.DocumentFiledType,
                groundRentPdfModel.AcknowledgementNumber,
                groundRentPdfModel.DateTimeFiled,
                groundRentPdfModel.PageAmount,
                groundRentPdfModel.Book,
                groundRentPdfModel.Page,
                groundRentPdfModel.ClerkInitials,
                groundRentPdfModel.YearRecorded
            };
            var dynParms = new DynamicParameters(parms);
            dynParms.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _unitOfWork.Connection.ExecuteAsync("spBaltimoreCity_CreateOrUpdateGroundRentPdf", dynParms, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            groundRentPdfModel.Id = dynParms.Get<int>("Id");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    public async Task<List<AddressModel>> ReadTopAmountWhereIsGroundRentNull(int amount)
    {
        return (await _unitOfWork.Connection.QueryAsync<AddressModel>("spBaltimoreCity_ReadTopAmountWhereIsGroundRentNull",
            new { Amount = amount },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction)).ToList();
    }
    public async Task<List<AddressModel>> ReadTopAmountWhereIsGroundRentTrue(int amount)
    {
        return (await _unitOfWork.Connection.QueryAsync<AddressModel>("spBaltimoreCity_ReadTopAmountWhereIsGroundRentTrue",
            new { Amount = amount },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction)).ToList();
    }
    public async Task<bool> Delete(string accountId)
    {
        try
        {
            await _unitOfWork.Connection.ExecuteAsync("spBaltimoreCity_Delete", new { AccountId = accountId },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}

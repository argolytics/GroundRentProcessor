﻿using DataLibrary.Models;
using Dapper;
using System.Data;
using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class HowardCountySqlDataService : IGroundRentProcessorDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public HowardCountySqlDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task CreateOrUpdateFile(AddressModel addressModel)
    {
        var parms = new
        {
            addressModel.AccountId,
            addressModel.AccountNumber,
            addressModel.Ward,
            addressModel.LandUseCode,
            addressModel.YearBuilt
        };
        await _unitOfWork.Connection.ExecuteAsync("spHowardCounty_CreateOrUpdateFile", parms,
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
    }
    public async Task CreateOrUpdateSDATRedeemedFile(AddressModel addressModel)
    {
        var parms = new
        {
            addressModel.AccountId,
            addressModel.IsRedeemed
        };
        await _unitOfWork.Connection.ExecuteAsync("spHowardCounty_CreateOrUpdateSDATRedeemedFile", parms,
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
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
            await _unitOfWork.Connection.ExecuteAsync("spHowardCounty_CreateOrUpdateSDATScraper", parms,
                commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
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
        return (await _unitOfWork.Connection.QueryAsync<AddressModel>("spHowardCounty_ReadTopAmountWhereIsGroundRentNull",
            new { Amount = amount },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction)).ToList();
    }
    public async Task<List<AddressModel>> ReadTopAmountWhereIsGroundRentTrue(int amount)
    {
        return (await _unitOfWork.Connection.QueryAsync<AddressModel>("spHowardCounty_ReadTopAmountWhereIsGroundRentTrue",
            new { Amount = amount },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction)).ToList();
    }
    public async Task<bool> Delete(string accountId)
    {
        try
        {
            await _unitOfWork.Connection.ExecuteAsync("spHowardCounty_Delete", new { AccountId = accountId },
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

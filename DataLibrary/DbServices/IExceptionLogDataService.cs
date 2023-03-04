using DataLibrary.Models;

namespace DataLibrary.DbServices;

public interface IExceptionLogDataService
{
    public Task Create(ExceptionLogModel exceptionLogModel);
    public Task<List<ExceptionLogModel>> ReadTopAmount(int amount);
    public Task<List<ExceptionLogModel>> ReadByCounty(string county);
    public Task<List<ExceptionLogModel>> ReadById(int id);
    public Task<List<ExceptionLogModel>> ReadByAccountId(string accountId);
    public Task<List<ExceptionLogModel>> ReadByGroundRentPdfId(string groundRentPdfId);
    public Task<bool> Delete(int id);
}
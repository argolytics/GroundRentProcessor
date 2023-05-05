using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class MONTDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new MONTSqlDataService(uow);
}

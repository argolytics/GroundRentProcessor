using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class WASHDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new WASHSqlDataService(uow);
}

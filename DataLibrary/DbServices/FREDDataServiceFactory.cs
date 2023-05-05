using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class FREDDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new FREDSqlDataService(uow);
}

using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class WICODataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new WICOSqlDataService(uow);
}

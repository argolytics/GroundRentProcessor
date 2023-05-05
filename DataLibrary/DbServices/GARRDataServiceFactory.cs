using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class GARRDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new GARRSqlDataService(uow);
}

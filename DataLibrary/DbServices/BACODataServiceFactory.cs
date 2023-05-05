using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class BACODataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new BACOSqlDataService(uow);
}

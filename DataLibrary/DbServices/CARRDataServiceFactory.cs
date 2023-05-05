using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class CARRDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new CARRSqlDataService(uow);
}

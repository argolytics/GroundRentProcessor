using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class KENTDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new KENTSqlDataService(uow);
}

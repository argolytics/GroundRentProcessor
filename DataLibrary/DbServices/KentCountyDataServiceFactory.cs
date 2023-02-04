using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class KentCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new KentCountySqlDataService(uow);
}

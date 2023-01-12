using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class BaltimoreCityDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new BaltimoreCitySqlDataService(uow);
}

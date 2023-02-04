using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class WashingtonCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new WashingtonCountySqlDataService(uow);
}

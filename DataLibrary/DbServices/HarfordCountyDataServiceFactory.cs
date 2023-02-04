using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class HarfordCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new HarfordCountySqlDataService(uow);
}

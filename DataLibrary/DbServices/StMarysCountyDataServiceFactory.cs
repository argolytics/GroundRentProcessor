using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class StMarysCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new StMarysCountySqlDataService(uow);
}

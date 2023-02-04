using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class TalbotCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new TalbotCountySqlDataService(uow);
}

using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class BaltimoreCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new BaltimoreCountySqlDataService(uow);
}

using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class SomersetCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new SomersetCountySqlDataService(uow);
}

using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class WorcesterCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new WorcesterCountySqlDataService(uow);
}

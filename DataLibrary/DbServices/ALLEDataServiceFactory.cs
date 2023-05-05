using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class ALLEDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new ALLESqlDataService(uow);
}

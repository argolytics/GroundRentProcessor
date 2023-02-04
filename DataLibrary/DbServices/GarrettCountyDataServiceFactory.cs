using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class GarrettCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new GarrettCountySqlDataService(uow);
}

using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class CarrollCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new CarrollCountySqlDataService(uow);
}

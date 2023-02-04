using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class CarolineCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new CarolineCountySqlDataService(uow);
}

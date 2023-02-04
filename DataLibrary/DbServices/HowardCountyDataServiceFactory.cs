using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class HowardCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new HowardCountySqlDataService(uow);
}

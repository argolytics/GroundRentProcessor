using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class CharlesCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new CharlesCountySqlDataService(uow);
}

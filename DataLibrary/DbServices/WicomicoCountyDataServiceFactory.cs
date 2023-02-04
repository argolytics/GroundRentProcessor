using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class WicomicoCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new WicomicoCountySqlDataService(uow);
}

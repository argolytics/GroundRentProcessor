using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class AlleganyCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new AlleganyCountySqlDataService(uow);
}

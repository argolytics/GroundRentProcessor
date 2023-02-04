using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class CalvertCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new CalvertCountySqlDataService(uow);
}

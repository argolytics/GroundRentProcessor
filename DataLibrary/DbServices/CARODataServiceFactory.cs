using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class CARODataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new CAROSqlDataService(uow);
}

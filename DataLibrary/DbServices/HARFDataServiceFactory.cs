using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class HARFDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new HARFSqlDataService(uow);
}

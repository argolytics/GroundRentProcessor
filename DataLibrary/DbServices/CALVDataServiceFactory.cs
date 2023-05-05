using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class CALVDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new CALVSqlDataService(uow);
}

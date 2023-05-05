using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class CHARDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new CHARSqlDataService(uow);
}

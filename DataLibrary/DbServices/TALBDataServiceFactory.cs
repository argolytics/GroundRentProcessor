using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class TALBDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new TALBSqlDataService(uow);
}

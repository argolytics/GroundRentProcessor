using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class QUEEDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new QUEESqlDataService(uow);
}

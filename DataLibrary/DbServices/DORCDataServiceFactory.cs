using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class DORCDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new DORCSqlDataService(uow);
}

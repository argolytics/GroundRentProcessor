using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class WORCDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new WORCSqlDataService(uow);
}

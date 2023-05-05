using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class HOWADataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new HOWASqlDataService(uow);
}

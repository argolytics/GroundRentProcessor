using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class BACIDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new BACISqlDataService(uow);
}

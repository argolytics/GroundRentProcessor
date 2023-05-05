using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class CECIDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new CECISqlDataService(uow);
}

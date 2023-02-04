using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class DorchesterCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new DorchesterCountySqlDataService(uow);
}

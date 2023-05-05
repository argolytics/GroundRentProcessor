using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class ANNEDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new ANNESqlDataService(uow);
}

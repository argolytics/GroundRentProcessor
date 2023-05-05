using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class SOMEDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new SOMESqlDataService(uow);
}

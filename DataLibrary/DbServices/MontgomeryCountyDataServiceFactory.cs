using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class MontgomeryCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new MontgomeryCountySqlDataService(uow);
}

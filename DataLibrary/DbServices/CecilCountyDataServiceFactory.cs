using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class CecilCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new CecilCountySqlDataService(uow);
}

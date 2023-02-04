using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class FrederickCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new FrederickCountySqlDataService(uow);
}

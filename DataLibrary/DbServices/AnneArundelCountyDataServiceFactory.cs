using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class AnneArundelCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new AnneArundelCountySqlDataService(uow);
}

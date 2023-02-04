using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class PrinceGeorgesCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new PrinceGeorgesCountySqlDataService(uow);
}

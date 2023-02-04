using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class QueenAnnesCountyDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new QueenAnnesCountySqlDataService(uow);
}

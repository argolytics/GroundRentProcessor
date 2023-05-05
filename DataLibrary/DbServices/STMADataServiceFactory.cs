using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class STMADataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new STMASqlDataService(uow);
}

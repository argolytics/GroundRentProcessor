using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class PRINDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new PRINSqlDataService(uow);
}

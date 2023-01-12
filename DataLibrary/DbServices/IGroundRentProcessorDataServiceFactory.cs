using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;
public interface IGroundRentProcessorDataServiceFactory
{
    IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow);
}
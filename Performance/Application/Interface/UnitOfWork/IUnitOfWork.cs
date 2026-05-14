namespace Performance.Application.Interface.UnitOfWork
{
    public interface IUnitOfWork :  IUnitOfWorkRepository, IUnitOfWorkTransaction, IDisposable
    {
        
    }
}

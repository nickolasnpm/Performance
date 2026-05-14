using Performance.Application.Interface.Repository;

namespace Performance.Application.Interface.UnitOfWork
{
    public interface IUnitOfWorkRepository
    {
        IUserRepositories UserRepository { get; }
        IAddressRepositories AddressRepository { get; }
    }
}
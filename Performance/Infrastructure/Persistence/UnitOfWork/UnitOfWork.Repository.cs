using Performance.Application.Interface.Repository;
using Performance.Application.Interface.UnitOfWork;
using Performance.Infrastructure.Persistence.Repositories;

namespace Performance.Infrastructure.Persistence.UnitOfWork
{
    public partial class UnitOfWork: IUnitOfWorkRepository
    {
        private IUserRepositories? _userRepository;
        private IAddressRepositories? _addressRepository;

        IUserRepositories IUnitOfWorkRepository.UserRepository
        {
            get { return _userRepository ??= new UserRepositories(context, appSettings, cacheSettings); }
        }

        IAddressRepositories IUnitOfWorkRepository.AddressRepository
        {
            get { return _addressRepository ??= new AddressRepositories(context); }
        }
    }
}
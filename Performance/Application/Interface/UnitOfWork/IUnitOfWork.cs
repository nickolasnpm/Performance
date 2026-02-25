using Performance.Application.Interface.Repository;
using Performance.Infrastructure.Repositories;

namespace Performance.Application.Interface.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // repositories
        IUserRepositories UserRepository { get; }

        // Transaction Management
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

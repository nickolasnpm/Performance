using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Performance.Application.Common.Settings;
using Performance.Application.Interface.Repository;
using Performance.Application.Interface.UnitOfWork;
using Performance.Infrastructure.Repositories;

namespace Performance.Infrastructure.UnitOfWork
{
    public class UnitOfWork(UserDbContext context, IOptions<AppSettings> appSettings, IOptions<CacheSettings> cacheSettings)
        : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;

        private IUserRepositories? _userRepository;

        #region repositories
        IUserRepositories IUnitOfWork.UserRepository
        {
            get { return _userRepository ??= new UserRepositories(context, appSettings, cacheSettings); }
        }
        #endregion

        #region transactions management
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
                throw new InvalidOperationException("A transaction is already in progress.");

            _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException("No active transaction to commit.");

            try
            {
                await context.SaveChangesAsync(cancellationToken);
                await _transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException("No active transaction to roll back.");

            try
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

        private async Task DisposeTransactionAsync()
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            context.Dispose();
        }
        #endregion
    }
}
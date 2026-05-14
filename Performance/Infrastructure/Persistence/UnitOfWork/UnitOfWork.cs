using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Performance.Application.Common.Settings;
using Performance.Application.Interface.UnitOfWork;

namespace Performance.Infrastructure.Persistence.UnitOfWork
{
    public partial class UnitOfWork(PerformanceDbContext context, IOptions<AppSettings> appSettings, IOptions<CacheSettings> cacheSettings)
        : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;
    }
}
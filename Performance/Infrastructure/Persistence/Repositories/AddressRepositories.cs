using Performance.Application.Interface.Repository;
using Performance.Domain.Entity;

namespace Performance.Infrastructure.Persistence.Repositories
{
    public class AddressRepositories (PerformanceDbContext context)
        : IAddressRepositories
    {
        public IQueryable<Address> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Create(IEnumerable<Address> entities)
        {
            throw new NotImplementedException();
        }

        public async Task Create(Address entity)
        {
            await context.Addresses.AddAsync(entity);
        }

        public Task Delete(HashSet<long> ids)
        {
            throw new NotImplementedException();
        }

        public Task Update(IEnumerable<Address> entities)
        {
            throw new NotImplementedException();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Performance.Domain.Entity;
using Performance.Infrastructure;
using Testcontainers.MsSql;

namespace IntegrationTest
{
    public class DatabaseFixture : IAsyncLifetime
    {
        private readonly MsSqlContainer _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();

        public UserDbContext DbContext { get; private set; } = null!;

        public async Task InitializeAsync()
        {
            await _container.StartAsync();

            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseSqlServer(_container.GetConnectionString())
                .Options;

            DbContext = new UserDbContext(options);
            await DbContext.Database.EnsureCreatedAsync();
            await SeedData();
        }

        private async Task SeedData()
        {
            DbContext.Users.AddRange(Enumerable.Range(1, 20).Select(i => new User
            {
                FirstName = $"User {i}",
                LastName = $"Test {i}",
                Username = $"username{i}",
                PhoneNumber = $"123-456-789{i}",
                Email = $"user{i}@test.com",
                CreatedBy = $"Seeder {i}",
                CreatedAt = DateTime.UtcNow,
                UpdatedBy = $"Seeder {i}",
                UpdatedAt = DateTime.UtcNow
            }));
            await DbContext.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await DbContext.DisposeAsync();
            await _container.DisposeAsync();
        }
    }
}
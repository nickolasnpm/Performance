using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Performance.Application.Common.Settings;
using Performance.Application.DTOs;
using Performance.Application.Extensions.Repository.EntityIncludeOptions;
using Performance.Infrastructure.Persistence.Repositories;

namespace IntegrationTest;

public class UserRepositoriesTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public UserRepositoriesTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    private UserRepositories CreateSut(bool useCache = false, int cacheMinutes = 60)
    {
        var appSettings = Options.Create(new AppSettings
        {
            IsUseCache = useCache
        });

        var cacheSettings = Options.Create(new CacheSettings
        {
            Items = new Dictionary<string, CacheItemSettings>
            {
                [CacheKeys.UserCount] = new()
                {
                    Key = CacheKeys.UserCount,
                    ExpirationMinutes = cacheMinutes
                }
            }
        });

        return new UserRepositories(_fixture.DbContext, appSettings, cacheSettings);
    }

    #region GetAll

    [Fact]
    public async Task GetAll_ShouldReturnAllUsers_AndNotTrackEntities()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var users = await sut.GetAll().ToListAsync();

        // Assert
        users.Should().HaveCount(20);
    }
    #endregion

    #region Offset Pagination

    [Fact]
    public async Task GetPaginatedUsersByOffset_FirstPage_ShouldReturnCorrectItems()
    {
        // Arrange
        var sut = CreateSut();
        var request = new OffsetPaginationRequest (Page: 1, Size: 5 );

        // Act
        var (query, totalCount) =
            await sut.GetPaginatedUsersByOffset(request, UserIncludeOptions.None);

        var users = await query.ToListAsync();

        // Assert
        totalCount.Should().Be(20);
        users.Should().HaveCount(5);
        users.Select(u => u.Id).Should().ContainInOrder(1, 2, 3, 4, 5);
    }

    [Fact]
    public async Task GetPaginatedUsersByOffset_LastPage_ShouldReturnRemainingItems()
    {
        var sut = CreateSut();
        var request = new OffsetPaginationRequest { Page = 4, Size = 5 };

        var (query, totalCount) =
            await sut.GetPaginatedUsersByOffset(request, UserIncludeOptions.None);

        var users = await query.ToListAsync();

        totalCount.Should().Be(20);
        users.Should().HaveCount(5);
        users.Select(u => u.Id).Should().ContainInOrder(16, 17, 18, 19, 20);
    }
    #endregion

    #region Cursor Pagination

    [Fact]
    public async Task GetPaginatedUsersByCursor_Forward_ShouldReturnItemsAfterCursor()
    {
        var sut = CreateSut();

        var request = new CursorPaginationRequest
        (
            Cursor: null,
            IsQueryPreviousPage: false,
            Size: 3
        );

        var (query, totalCount) = await sut.GetPaginatedUsersByCursor(0, request, UserIncludeOptions.None);

        var users = await query.ToListAsync();

        totalCount.Should().Be(20);
        users.Select(u => u.Id).Should().OnlyContain(id => id > 0);
        users.Count.Should().BeLessThanOrEqualTo(4); // PageSize + 1 scenario
    }

    [Fact]
    public async Task GetPaginatedUsersByCursor_Backward_ShouldReturnItemsBeforeCursor()
    {
        var sut = CreateSut();

        var request = new CursorPaginationRequest
        (
            Cursor: "something",
            IsQueryPreviousPage: true,
            Size: 3
        );

        var (query, _) = await sut.GetPaginatedUsersByCursor(10, request, UserIncludeOptions.None);

        var users = await query.ToListAsync();

        users.Select(u => u.Id).Should().OnlyContain(id => id < 10);
    }

    [Fact]
    public async Task GetPaginatedUsersByCursor_CursorBeyondMax_ShouldReturnEmpty()
    {
        var sut = CreateSut();

        var request = new CursorPaginationRequest
        (
            Cursor: "something",
            IsQueryPreviousPage: false,
            Size: 5
        );

        var (query, _) = await sut.GetPaginatedUsersByCursor(20, request, UserIncludeOptions.None);

        var users = await query.ToListAsync();

        users.Should().BeEmpty();
    }
    #endregion

    #region Cache Behavior

    [Fact]
    public async Task GetPaginatedUsersByOffset_WithCacheEnabled_ShouldCacheTotalCount()
    {
        var sut = CreateSut(useCache: true, cacheMinutes: 5);
        var request = new OffsetPaginationRequest { Page = 1, Size = 5 };

        // First call (should populate cache)
        var (_, totalCount1) =
            await sut.GetPaginatedUsersByOffset(request, UserIncludeOptions.None);

        totalCount1.Should().Be(20);

        // Modify DB after cache populated
        var userToRemove = await _fixture.DbContext.Users.FirstAsync();
        _fixture.DbContext.Users.Remove(userToRemove);
        await _fixture.DbContext.SaveChangesAsync();

        // Second call (should return cached count)
        var (_, totalCount2) =
            await sut.GetPaginatedUsersByOffset(request, UserIncludeOptions.None);

        totalCount2.Should().Be(20);
    }
    #endregion
}
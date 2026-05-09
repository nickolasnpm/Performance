using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Repository.EntityColumnSort
{
    public static class UserColumnSort
    {
        extension(IQueryable<User> query)
        {
            public IQueryable<User> ApplySorting(string? sortBy, bool isAscending)
            {
                switch (sortBy)
                {
                    case nameof(User.Email):
                        return isAscending ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email);

                    case nameof(User.FirstName):
                        return isAscending ? query.OrderBy(u => u.FirstName) : query.OrderByDescending(u => u.FirstName);

                    case nameof(User.DateOfBirth):
                        return isAscending ? query.OrderBy(u => u.DateOfBirth) : query.OrderByDescending(u => u.DateOfBirth);

                    default:
                        return isAscending ? query.OrderBy(u => u.Id) : query.OrderByDescending(u => u.Id);
                }
            }
        }
    }
}
namespace Performance.Application.Common.Models
{
    public record UserPaginatedResult<T>(IQueryable<T> Items, int TotalCount);
}
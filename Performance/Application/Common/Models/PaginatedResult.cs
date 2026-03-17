namespace Performance.Application.Common.Models
{
    public record PaginatedResult<T>(IQueryable<T> Items, int TotalCount);
}
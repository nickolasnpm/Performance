using Microsoft.EntityFrameworkCore;
using Performance.Application.Common;
using Performance.Application.DTOs;
using Performance.Application.Interface.Services;
using Performance.Application.Interface.UnitOfWork;
using Performance.Application.Extensions.Repository;
using Performance.Domain.Entity;

namespace Performance.Domain.Services
{
    public class UserServices (IUnitOfWork unitOfWork)
        : IUserServices
    {
        public IQueryable<User> GetAllAsync()
        {
            return unitOfWork.UserRepository.GetAll();
        }

        public async Task<UserResponseDTO<User>> GetPaginatedListAsync(UserRequestDTO request)
        {
            switch (request.PaginationType)
            {
                case PaginationType.Offset:
                    if (request.OffsetPagination is null)
                       throw new ArgumentException("Offset pagination request is required.");

                    return await OffsetPaginationAsync(request.OffsetPagination);

                case PaginationType.Cursor:
                    if (request.CursorPagination is null)
                       throw new ArgumentException("Cursor pagination request is required.");

                    return await CursorPaginationAsync(request.CursorPagination);

                default:
                    throw new ArgumentException("Invalid pagination type.");
            }
        }

        private async Task<OffsetPaginationResponse<User>> OffsetPaginationAsync(OffsetPaginationRequest request)
        {
            var (users, totalCount) = await unitOfWork.UserRepository.GetPaginatedUsersByOffset(request, UserIncludeOptions.All);
            //var (users, totalCount) = await _offsetRepository.GetPaginatedUsersByCursor(request, new UserIncludeOptions() { Address = true });

            int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            bool hasNextPage = request.Page < totalPages;
            bool hasPreviousPage = request.Page > 1;

            return new OffsetPaginationResponse<User>()
            {
                Data = await users.ToListAsync(),
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage
            };
        }

        private async Task<CursorPaginationResponse<User>> CursorPaginationAsync(CursorPaginationRequest request)
        {
            var (users, totalCount) = await unitOfWork.UserRepository.GetPaginatedUsersByCursor(request, UserIncludeOptions.All);
            var result = await users.ToListAsync();

            bool hasMore = result.Count > request.PageSize;

            if (hasMore)
                result.RemoveAt(result.Count - 1);

            bool hasNextPage;
            bool hasPreviousPage;

            if (request.IsQueryPreviousPage)
            {
                result.Reverse();
                hasNextPage = true;
                hasPreviousPage = hasMore;
            }
            else
            {
                hasNextPage = hasMore;
                hasPreviousPage = request.Cursor > 0;
            }

            long? nextCursor = hasNextPage ? result.Last().Id : null;
            long? previousCursor = hasPreviousPage ? result.First().Id : null;

            return new CursorPaginationResponse<User>
            {
                Data = result,
                TotalCount = totalCount, // optional
                NextCursor = nextCursor,
                PreviousCursor = previousCursor,
            };
        }
    }
}

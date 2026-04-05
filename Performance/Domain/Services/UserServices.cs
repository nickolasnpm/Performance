using Microsoft.EntityFrameworkCore;
using Performance.Application.Interface.Services;
using Performance.Application.Interface.UnitOfWork;
using Performance.Application.Extensions.Repository.EntityIncludeOptions;
using Performance.Domain.Entity;
using Performance.Application.DTOs.Users;
using Performance.Application.Common.Enums;
using Performance.Application.Extensions.Mapping.Users;
using Microsoft.AspNetCore.Identity;
using Performance.Application.Extensions.Mapping;

namespace Performance.Domain.Services
{
    public class UserServices (IUnitOfWork unitOfWork)
        : IUserServices
    {
        public IQueryable<User> GetAllAsync()
        {
            return unitOfWork.UserRepository.GetAll();
        }

        public async Task<UserResponseDTO<UserDTO>> GetPaginatedListAsync(UserRequestDTO request)
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

        private async Task<OffsetPaginationResponse<UserDTO>> OffsetPaginationAsync(OffsetPaginationRequest request)
        {
            var (users, totalCount) = await unitOfWork.UserRepository.GetPaginatedUsersByOffset(request, UserIncludeOptions.All);
            //var (users, totalCount) = await _offsetRepository.GetPaginatedUsersByCursor(request, new UserIncludeOptions() { Address = true });

            int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            bool hasNextPage = request.Page < totalPages;
            bool hasPreviousPage = request.Page > 1;

            // var result = await users.ToListAsync();

            return new OffsetPaginationResponse<UserDTO>()
            {
                Data = users.MapEntityToDTO(UserMapper.ToDTO),
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage
            };
        }

        private async Task<CursorPaginationResponse<UserDTO>> CursorPaginationAsync(CursorPaginationRequest request)
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

            return new CursorPaginationResponse<UserDTO>
            {
                Data = result.MapEntityToDTO(UserMapper.ToDTO),
                TotalCount = totalCount, // optional
                NextCursor = nextCursor,
                PreviousCursor = previousCursor,
            };
        }
    }
}

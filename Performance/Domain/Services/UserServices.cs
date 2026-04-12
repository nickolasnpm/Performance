using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Diagnostics.Tracing.StackSources;
using Microsoft.EntityFrameworkCore;
using Performance.Application.Common.Enums;
using Performance.Application.Common.Models;
using Performance.Application.DTOs.Users;
using Performance.Application.Extensions.Mapping;
using Performance.Application.Extensions.Mapping.Users;
using Performance.Application.Extensions.Repository.EntityIncludeOptions;
using Performance.Application.Interface.Services;
using Performance.Application.Interface.UnitOfWork;
using Performance.Domain.Entity;
using Performance.Infrastructure;

namespace Performance.Domain.Services
{
    public class UserServices (IUnitOfWork unitOfWork)
        : IUserServices
    {
        private const int MaxBatchSize = 500;
        private readonly string MaxBatchSizeErrorResponse = $"Batch size cannot exceed {MaxBatchSize}";

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

        public async Task<User?> GetByIdAsync(long Id)
        {
            return await GetAllAsync().FirstOrDefaultAsync(u => u.Id == Id);
        }

        public async Task<Result<bool, List<AddErrorResponseDTO>>> CreateUsers(List<AddUserRequestDTO> requestDTOs)
        {
            if (requestDTOs.Count > MaxBatchSize)
                throw new ArgumentException(MaxBatchSizeErrorResponse);

            var requestedUsernames = requestDTOs.Select(u => u.Username).ToHashSet();
            var requestedEmails = requestDTOs.Select(u => u.Email).ToHashSet();

            var existingUsers = await GetAllAsync()
                .Where(u => requestedUsernames.Contains(u.Username) || requestedEmails.Contains(u.Email))
                .Select(u => new { u.Username, u.Email })
                .ToListAsync();

            var existingUsernames = existingUsers.Select(u => u.Username).ToHashSet();
            var existingEmails = existingUsers.Select(u => u.Email).ToHashSet();

            var errors = requestDTOs
                .Select(u => new AddErrorResponseDTO
                {
                    Username = u.Username,
                    Email = u.Email,
                    IsUsernameExist = existingUsernames.Contains(u.Username),
                    IsEmailExist = existingEmails.Contains(u.Email)
                })
                .Where(e => e.IsUsernameExist || e.IsEmailExist)
                .ToList();

            if (errors.Any())
                return Result<bool, List<AddErrorResponseDTO>>.Failure(errors);

            var toBeCreated = requestDTOs.MapDTOToEntity(UserMapper.AddRequestToEntity);
            await unitOfWork.UserRepository.Create(toBeCreated);
            await unitOfWork.SaveChangesAsync();

            return Result<bool, List<AddErrorResponseDTO>>.Success(true);
        }

        public async Task<Result<bool, List<long>>> UpdateUsers(List<UpdateUserRequestDTO> requestDTOs)
        {
            if (requestDTOs.Count > MaxBatchSize)
                throw new ArgumentException(MaxBatchSizeErrorResponse);

            HashSet<long> entityIds = requestDTOs.Select(u => u.Id).ToHashSet();

            var existingUsers = await GetAllAsync()
                .AsTracking()
                .Where(u => entityIds.Contains(u.Id))
                .ToListAsync();

            var existingIds = existingUsers.Select(u => u.Id).ToHashSet();
            var notfoundIds = entityIds.Except(existingIds).ToList();

            if (notfoundIds.Any())
                return Result<bool, List<long>>.Failure(notfoundIds);

            var existingUsersById = existingUsers.ToDictionary(u => u.Id);
            requestDTOs.MapDTOToEntity(dto => existingUsersById[dto.Id], UserMapper.UpdateRequestToEntity);

            await unitOfWork.SaveChangesAsync();

            return Result<bool, List<long>>.Success(true);
        }

        public async Task<Result<bool, List<long>>> DeleteUsers(HashSet<long> ids)
        {
            if (ids.Count > MaxBatchSize)
                throw new ArgumentException(MaxBatchSizeErrorResponse);

            var existingIds = await GetAllAsync()
                .Where(u => ids.Contains(u.Id))
                .Select(u => u.Id).ToHashSetAsync();

            var notfoundids = ids.Except(existingIds).ToList();

            if (notfoundids.Any())
                return Result<bool, List<long>>.Failure(notfoundids);

            await unitOfWork.UserRepository.Delete(existingIds);

            return Result<bool, List<long>>.Success(true);
        }

        #region private methods
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
                Data = users.MapEntityToDTO(UserMapper.EntityToDTO),
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
                Data = result.MapEntityToDTO(UserMapper.EntityToDTO),
                TotalCount = totalCount, // optional
                NextCursor = nextCursor,
                PreviousCursor = previousCursor,
            };
        }
        #endregion
    }
}

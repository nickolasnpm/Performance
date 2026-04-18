using Microsoft.EntityFrameworkCore;
using Performance.Application.Common.Enums;
using Performance.Application.Common.Models;
using Performance.Application.DTOs.Users;
using Performance.Application.Extensions.Mapping;
using Performance.Application.Extensions.Mapping.Users;
using Performance.Application.Extensions.Repository.EntityIncludeOptions;
using Performance.Application.Interface.Hashing;
using Performance.Application.Interface.Services;
using Performance.Application.Interface.UnitOfWork;

namespace Performance.Application.Services
{
    public class UserServices(IUnitOfWork unitOfWork, IIdHelper idHelper)
        : IUserServices
    {
        private const int MaxBatchSize = 500;
        private static readonly string MaxBatchSizeErrorResponse = $"Batch size cannot exceed {MaxBatchSize}";
        public async Task<Result<ListResponseDTO<UserDTO>, ResultError>> GetPaginatedListAsync(ListRequestDTO request)
        {
            switch (request.PaginationType)
            {
                case PaginationType.Offset:
                    if (request.OffsetPagination is null)
                        return Result<ListResponseDTO<UserDTO>, ResultError>.Failure(new ResultError
                        { ErrorType = ErrorType.ValidationError, Message = "Offset pagination request is required." });

                    return Result<ListResponseDTO<UserDTO>, ResultError>.Success(await OffsetPaginationAsync(request.OffsetPagination));

                case PaginationType.Cursor:
                    if (request.CursorPagination is null)
                        return Result<ListResponseDTO<UserDTO>, ResultError>.Failure(new ResultError
                        { ErrorType = ErrorType.ValidationError, Message = "Cursor pagination request is required." });

                    return Result<ListResponseDTO<UserDTO>, ResultError>.Success(await CursorPaginationAsync(request.CursorPagination));

                default:
                    return Result<ListResponseDTO<UserDTO>, ResultError>.Failure(new ResultError
                    { ErrorType = ErrorType.ValidationError, Message = "Invalid pagination type." });
            }
        }

        public async Task<Result<UserDTO, ResultError>> GetByIdAsync(long id)
        {
            var user = await unitOfWork.UserRepository.GetAll()
                .Where(u => u.Id == id)
                .Select(u => UserMapper.EntityToDTO(u, idHelper))
                .FirstOrDefaultAsync();

            return user != null
                ? Result<UserDTO, ResultError>.Success(user)
                : Result<UserDTO, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.NotFound, Message = "User not found." });
        }

        public async Task<Result<bool, ResultError>> CreateUsers(List<AddUserRequestDTO> requestDTOs)
        {
            if (requestDTOs.Count > MaxBatchSize)
                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.BatchSizeExceeded, Message = MaxBatchSizeErrorResponse });

            var duplicatesInBatch = requestDTOs.GroupBy(u => u.Username, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            var duplicateEmailsInBatch = requestDTOs.GroupBy(u => u.Email, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicatesInBatch.Any() || duplicateEmailsInBatch.Any())
            {
                var dataDuplicatedErrors = requestDTOs
                    .Where(u => duplicatesInBatch.Contains(u.Username, StringComparer.OrdinalIgnoreCase)
                                || duplicateEmailsInBatch.Contains(u.Email, StringComparer.OrdinalIgnoreCase))
                    .Select(u => new AddErrorResponseDTO
                    {
                        Username = u.Username,
                        Email = u.Email,
                        IsUsernameExist = duplicatesInBatch.Contains(u.Username, StringComparer.OrdinalIgnoreCase),
                        IsEmailExist = duplicateEmailsInBatch.Contains(u.Email, StringComparer.OrdinalIgnoreCase)
                    })
                    .ToList();

                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.Conflict, Message = "Some usernames or emails are duplicated in the request.", Payload = dataDuplicatedErrors });
            }

            var requestedUsernames = requestDTOs.Select(u => u.Username).ToHashSet();
            var requestedEmails = requestDTOs.Select(u => u.Email).ToHashSet();

            var existingUsers = await unitOfWork.UserRepository.GetAll()
                .Where(u => requestedUsernames.Contains(u.Username) || requestedEmails.Contains(u.Email))
                .Select(u => new { u.Username, u.Email })
                .ToListAsync();

            var existingUsernames = existingUsers.Select(u => u.Username).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var existingEmails = existingUsers.Select(u => u.Email).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var dataExistedErrors = requestDTOs
                .Select(u => new AddErrorResponseDTO
                {
                    Username = u.Username,
                    Email = u.Email,
                    IsUsernameExist = existingUsernames.Contains(u.Username),
                    IsEmailExist = existingEmails.Contains(u.Email)
                })
                .Where(e => e.IsUsernameExist || e.IsEmailExist)
                .ToList();

            if (dataExistedErrors.Any())
                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.Conflict, Message = "Some usernames or emails already exist.", Payload = dataExistedErrors });

            var toBeCreated = requestDTOs.Map(UserMapper.AddRequestToEntity);
            await unitOfWork.UserRepository.Create(toBeCreated);
            await unitOfWork.SaveChangesAsync();

            return Result<bool, ResultError>.Success(true);
        }

        public async Task<Result<bool, ResultError>> UpdateUsers(List<UpdateUserRequestDTO> requestDTOs)
        {
            if (requestDTOs.Count > MaxBatchSize)
                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.BatchSizeExceeded, Message = MaxBatchSizeErrorResponse });

            HashSet<long> entityIds = requestDTOs.Select(u => u.Id).ToHashSet();

            var existingUsers = await unitOfWork.UserRepository.GetAll()
                .AsTracking()
                .Where(u => entityIds.Contains(u.Id))
                .ToListAsync();

            var existingIds = existingUsers.Select(u => u.Id).ToHashSet();
            var notFoundIds = entityIds.Except(existingIds).ToList();

            if (notFoundIds.Any())
                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.NotFound, Message = "Some users are not found.", Payload = notFoundIds });

            var existingUsersById = existingUsers.ToDictionary(u => u.Id);
            requestDTOs.ForEach(dto => UserMapper.UpdateRequestToEntity(existingUsersById[dto.Id], dto));

            await unitOfWork.SaveChangesAsync();

            return Result<bool, ResultError>.Success(true);
        }

        public async Task<Result<bool, ResultError>> DeleteUsers(HashSet<long> ids)
        {
            if (ids.Count > MaxBatchSize)
                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.BatchSizeExceeded, Message = MaxBatchSizeErrorResponse });

            var existingIds = await unitOfWork.UserRepository.GetAll()
                .Where(u => ids.Contains(u.Id))
                .Select(u => u.Id).ToHashSetAsync();

            var notFoundIds = ids.Except(existingIds).ToList();

            if (notFoundIds.Any())
                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.NotFound, Message = "Some users not found.", Payload = notFoundIds });

            await unitOfWork.UserRepository.Delete(existingIds);

            return Result<bool, ResultError>.Success(true);
        }

        #region private methods
        private async Task<OffsetPaginationResponse<UserDTO>> OffsetPaginationAsync(OffsetPaginationRequest request)
        {
            var (users, totalCount) = await unitOfWork.UserRepository.GetPaginatedUsersByOffset(request, UserIncludeOptions.All);

            int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            bool hasNextPage = request.Page < totalPages;
            bool hasPreviousPage = request.Page > 1;

            return new OffsetPaginationResponse<UserDTO>()
            {
                Data = users.Map(u => UserMapper.EntityToDTO(u, idHelper)),
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
                Data = result.Select(u => UserMapper.EntityToDTO(u, idHelper)).ToList(),
                TotalCount = totalCount, // optional
                NextCursor = nextCursor,
                PreviousCursor = previousCursor,
            };
        }
        #endregion
    }
}

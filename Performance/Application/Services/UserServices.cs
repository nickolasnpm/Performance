using Microsoft.EntityFrameworkCore;
using Performance.Application.Common.Enums;
using Performance.Application.Common.Models;
using Performance.Application.DTOs;
using Performance.Application.DTOs.Users;
using Performance.Application.Extensions.Mapping;
using Performance.Application.Extensions.Repository.EntityIncludeOptions;
using Performance.Application.Interface.Security;
using Performance.Application.Interface.Services;
using Performance.Application.Interface.UnitOfWork;
using Performance.Domain.Entity;

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

        public async Task<Result<UserDTO, ResultError>> GetByIdAsync(string hashId)
        {
            var id = idHelper.DecryptId(hashId);

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

            // HashSet is unique by default. Adding duplicated name will return false
            var requestedUsernames = new HashSet<string>(requestDTOs.Count, StringComparer.OrdinalIgnoreCase);
            var requestedEmails = new HashSet<string>(requestDTOs.Count, StringComparer.OrdinalIgnoreCase);
            var duplicateUsernames = new HashSet<string>(requestDTOs.Count, StringComparer.OrdinalIgnoreCase);
            var duplicateEmails = new HashSet<string>(requestDTOs.Count, StringComparer.OrdinalIgnoreCase);

            foreach (var dto in requestDTOs)
            {
                if (!requestedUsernames.Add(dto.Username))
                    duplicateUsernames.Add(dto.Username);

                if (!requestedEmails.Add(dto.Email))
                    duplicateEmails.Add(dto.Email);
            }

            if (duplicateUsernames.Any() || duplicateEmails.Any())
            {
                var dataDuplicatedErrors = requestDTOs
                    .Where(u => duplicateUsernames.Contains(u.Username, StringComparer.OrdinalIgnoreCase)
                                || duplicateEmails.Contains(u.Email, StringComparer.OrdinalIgnoreCase))
                    .Select(u => new AddUserErrorResponseDTO
                    (
                        u.Username,
                        IsUsernameDuplicated: duplicateUsernames.Contains(u.Username, StringComparer.OrdinalIgnoreCase),
                        IsUsernameExist: null,
                        u.Email,
                        IsEmailDuplicated: duplicateEmails.Contains(u.Email, StringComparer.OrdinalIgnoreCase),
                        IsEmailExist: null
                    ))
                    .ToList();

                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.Conflict, Message = "Some usernames or emails are duplicated in the request.", Payload = dataDuplicatedErrors });
            }

            var existingUsers = new List<(string Username, string Email)>();

            foreach (var chunk in requestDTOs.Chunk(50))
            {
                var usernames = chunk.Select(x => x.Username).ToList();
                var emails = chunk.Select(x => x.Email).ToList();

                var batch = await unitOfWork.UserRepository.GetAll()
                    .Where(u => usernames.Contains(u.Username) || emails.Contains(u.Email))
                    .Select(u => new { u.Username, u.Email })
                    .ToListAsync();

                existingUsers.AddRange(batch.Select(u => (u.Username, u.Email)));
            }

            if (existingUsers.Count > 0)
            {
                var existingUsernames = new HashSet<string>(existingUsers.Count, StringComparer.OrdinalIgnoreCase);
                var existingEmails = new HashSet<string>(existingUsers.Count, StringComparer.OrdinalIgnoreCase);

                foreach (var u in existingUsers)
                {
                    existingUsernames.Add(u.Username);
                    existingEmails.Add(u.Email);
                }

                var dataExistedErrors = existingUsers
                    .Select(u => new AddUserErrorResponseDTO
                    (
                        u.Username,
                        IsUsernameDuplicated: null,
                        IsUsernameExist: existingUsernames.Contains(u.Username),
                        u.Email,
                        IsEmailDuplicated: null,
                        IsEmailExist: existingEmails.Contains(u.Email)
                    ))
                    .ToList();

                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.Conflict, Message = "Some usernames or emails already exist.", Payload = dataExistedErrors });
            }

            var toBeCreated = requestDTOs.Select(UserMapper.AddRequestToEntity).ToList();
            await unitOfWork.UserRepository.Create(toBeCreated);

            return Result<bool, ResultError>.Success(true);
        }

        public async Task<Result<bool, ResultError>> UpdateUsers(List<UpdateUserRequestDTO> requestDTOs)
        {
            if (requestDTOs.Count > MaxBatchSize)
                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.BatchSizeExceeded, Message = MaxBatchSizeErrorResponse });

            var duplicateEncryptedIds = requestDTOs
                .GroupBy(dto => idHelper.DecryptId(dto.Id))
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.Select(dto => dto.Id))
                .ToList();

            if (duplicateEncryptedIds.Any())
                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.ValidationError, Message = "Duplicate user IDs in request.", Payload = duplicateEncryptedIds });

            var dtoById = new Dictionary<long, UpdateUserRequestDTO>(requestDTOs.Count);

            foreach (var dto in requestDTOs)
                dtoById[idHelper.DecryptId(dto.Id)] = dto;

            var existingUsersById = await unitOfWork.UserRepository.GetAll()
                .AsTracking()
                .Where(u => dtoById.Keys.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var notFoundIds = dtoById.Keys.Except(existingUsersById.Keys).ToList();

            if (notFoundIds.Any())
            {
                var notFoundEncryptedIds = notFoundIds
                    .Select(id => dtoById[id].Id)
                    .ToList();

                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.NotFound, Message = "Some users are not found.", Payload = notFoundEncryptedIds });
            }

            foreach (var (id, dto) in dtoById)
                dto.UpdateRequestToEntity(existingUsersById[id]);

            await unitOfWork.UserRepository.Update(existingUsersById.Values);

            return Result<bool, ResultError>.Success(true);
        }

        public async Task<Result<bool, ResultError>> DeleteUsers(HashSet<string> ids)
        {
            if (ids.Count > MaxBatchSize)
                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.BatchSizeExceeded, Message = MaxBatchSizeErrorResponse });

            HashSet<long> entityIds = ids.Select(idHelper.DecryptId).ToHashSet();

            var existingIds = await unitOfWork.UserRepository.GetAll()
                .Where(u => entityIds.Contains(u.Id))
                .Select(u => u.Id).ToHashSetAsync();

            var notFoundIds = entityIds.Except(existingIds).ToList();

            if (notFoundIds.Any())
                return Result<bool, ResultError>.Failure(new ResultError
                { ErrorType = ErrorType.NotFound, Message = "Some users not found.", Payload = notFoundIds });

            await unitOfWork.UserRepository.Delete(existingIds);

            return Result<bool, ResultError>.Success(true);
        }

        #region private methods
        private async Task<ListResponseDTO<UserDTO>> OffsetPaginationAsync(OffsetPaginationRequest request)
        {
            var (users, totalCount) = await unitOfWork.UserRepository.GetPaginatedUsersByOffset(request, UserIncludeOptions.All);

            int totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

            bool hasNextPage = request.Page < totalPages;
            bool hasPreviousPage = request.Page > 1;

            return new ListResponseDTO<UserDTO>(
                Data: users.Select(u => u.EntityToDTO(idHelper)).ToList(),
                OffsetPaginationResponse: new OffsetPaginationResponse(
                    TotalCount: totalCount,
                    TotalPages: totalPages,
                    HasNextPage: hasNextPage,
                    HasPreviousPage: hasPreviousPage
                ),
                CursorPaginationResponse: null
            );
        }

        private async Task<ListResponseDTO<UserDTO>> CursorPaginationAsync(CursorPaginationRequest request)
        {
            long cursorValue = 0;

            if (!string.IsNullOrEmpty(request.Cursor))
                cursorValue = idHelper.DecryptId(request.Cursor);

            var (users, totalCount) = await unitOfWork.UserRepository.GetPaginatedUsersByCursor(cursorValue, request, UserIncludeOptions.All);
            var result = await users.ToListAsync();

            bool hasMore = result.Count > request.Size;

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
                hasPreviousPage = cursorValue > 0;
            }

            long? nextCursor = hasNextPage ? result.Last().Id : null;
            long? previousCursor = hasPreviousPage ? result.First().Id : null;

            return new ListResponseDTO<UserDTO>(
                Data: result.Select(u => u.EntityToDTO(idHelper)).ToList(),
                OffsetPaginationResponse: null,
                CursorPaginationResponse: new CursorPaginationResponse(
                    TotalCount: totalCount, // optional
                    NextCursor: nextCursor,
                    PreviousCursor: previousCursor
                )
            );
        }
        #endregion
    }
}
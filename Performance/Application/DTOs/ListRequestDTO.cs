using System.ComponentModel.DataAnnotations;
using Performance.Application.Common.Enums;

namespace Performance.Application.DTOs
{
    public record ListRequestDTO(
        [EnumDataType(typeof(PaginationType))]
        PaginationType PaginationType,
        OffsetPaginationRequest? OffsetPagination,
        CursorPaginationRequest? CursorPagination
    );

    public record OffsetPaginationRequest(
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        int Page = 1,
        int Size = 100
    );

    public record CursorPaginationRequest(
        string? Cursor,
        bool IsQueryPreviousPage = false,
        int Size = 100
    );
}
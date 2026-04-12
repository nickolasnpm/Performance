using System.ComponentModel.DataAnnotations;
using Performance.Application.Common.Enums;

namespace Performance.Application.DTOs.Users
{
    public class UserRequestDTO
    {
        [EnumDataType(typeof(PaginationType))]
        public PaginationType PaginationType { get; set; }
        public OffsetPaginationRequest? OffsetPagination { get; set; }
        public CursorPaginationRequest? CursorPagination { get; set; }
    }

    public class PaginationRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page size must be greater than 0.")]
        public int PageSize { get; set; } = 50;
    }

    public class OffsetPaginationRequest : PaginationRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int Page { get; set; } = 1;
    }

    public class CursorPaginationRequest : PaginationRequest
    {
        [Range(0, long.MaxValue, ErrorMessage = "Cursor must be a non-negative number.")]
        public long Cursor { get; set; } = 0;
        public bool IsQueryPreviousPage { get; set; } = false;
    }
}

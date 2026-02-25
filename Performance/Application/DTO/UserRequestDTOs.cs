using Performance.Application.Common;

namespace Performance.Application.DTO
{
    public class UserRequestDTO
    {
        public PaginationType PaginationType { get; set; }
        
        public OffsetPaginationRequest? OffsetPagination { get; set; }
        public CursorPaginationRequest? CursorPagination { get; set; }
    }

    public class PaginationRequest
    {
        public int PageSize { get; set; } = 50;
    }

    public class OffsetPaginationRequest : PaginationRequest
    {
        public int Page { get; set; } = 1;
    }

    public class CursorPaginationRequest : PaginationRequest
    {
        public long Cursor { get; set; } = 0;
        public bool IsQueryPreviousPage { get; set; } = false;
    }
}

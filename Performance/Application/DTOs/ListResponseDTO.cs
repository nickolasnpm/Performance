namespace Performance.Application.DTOs
{

    public record ListResponseDTO<T>(
        List<T> Data,
        OffsetPaginationResponse? OffsetPaginationResponse,
        CursorPaginationResponse? CursorPaginationResponse
    );

    public record OffsetPaginationResponse(
        int TotalCount,
        int TotalPages,
        bool HasNextPage,
        bool HasPreviousPage
    );

    public record CursorPaginationResponse(
        int TotalCount,
        long? NextCursor,
        long? PreviousCursor
    );
}

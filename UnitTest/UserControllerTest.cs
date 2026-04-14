using Microsoft.AspNetCore.Mvc;
using Moq;
using Performance.API.Controllers;
using Performance.Application.Common.Enums;
using Performance.Application.Common.Models;
using Performance.Application.DTOs.Users;
using Performance.Application.Interface.Services;

namespace UnitTest
{
    public class UserControllerTests
    {
        private readonly Mock<IUserServices> _mockUserServices;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserServices = new Mock<IUserServices>();
            _controller = new UserController(_mockUserServices.Object);
        }

        #region Test Data

        public static IEnumerable<object[]> PaginationTestData =>
        [
            [
                new ListRequestDTO
                {
                    PaginationType = PaginationType.Offset,
                    OffsetPagination = new OffsetPaginationRequest { PageSize = 100, Page = 1 }
                },
                new OffsetPaginationResponse<UserDTO>
                {
                    Data = new List<UserDTO>(),
                    TotalCount = 0,
                    TotalPages = 1000,
                    HasNextPage = true,
                    HasPreviousPage = false
                }
            ],
            [
                new ListRequestDTO
                {
                    PaginationType = PaginationType.Cursor,
                    CursorPagination = new CursorPaginationRequest { PageSize = 100, Cursor = 0, IsQueryPreviousPage = false }
                },
                new CursorPaginationResponse<UserDTO>
                {
                    Data = new List<UserDTO>(),
                    TotalCount = 0,
                    NextCursor = 100,
                    PreviousCursor = 1,
                }
            ]
        ];

        #endregion


        #region GetPaginatedUsers

        [Theory]
        [MemberData(nameof(PaginationTestData))]
        public async Task GetPaginatedUsers_ValidRequest_ReturnsOkWithResult(ListRequestDTO userRequestDTO, ListResponseDTO<UserDTO> expectedResultDTO)
        {
            // Arrange
            _mockUserServices
                .Setup(s => s.GetPaginatedListAsync(It.IsAny<ListRequestDTO>()))
                .ReturnsAsync(Result<ListResponseDTO<UserDTO>, ResultError>.Success(expectedResultDTO));

            // Act
            var result = await _controller.GetPaginatedUsers(userRequestDTO);

            // Assert
            _mockUserServices.Verify(s => s.GetPaginatedListAsync(It.IsAny<ListRequestDTO>()), Times.Once);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResultDTO, okResult.Value);
        }

        #endregion
    }
}
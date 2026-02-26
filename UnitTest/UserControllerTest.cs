using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Performance.API.Controllers;
using Performance.Application.Common;
using Performance.Application.DTO;
using Performance.Application.Interface.Services;
using Performance.Domain.Entity;

namespace UnitTest
{
    public class UserControllerTests
    {
        private readonly Mock<IUserServices> _mockUserServices;
        private readonly Mock<ILogger<UserController>> _mockLogger;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserServices = new Mock<IUserServices>();
            _mockLogger = new Mock<ILogger<UserController>>();
            _controller = new UserController(_mockUserServices.Object, _mockLogger.Object);
        }

        #region Test Data

        public static IEnumerable<object[]> PaginationTestData =>
        [
            [
                new UserRequestDTO
                {
                    PaginationType = PaginationType.Offset,
                    OffsetPagination = new OffsetPaginationRequest { PageSize = 100, Page = 1 }
                },
                new OffsetPaginationResponse<User>
                {
                    Data = new List<User>(),
                    TotalCount = 0,
                    TotalPages = 1000,
                    HasNextPage = true,
                    HasPreviousPage = false
                }
            ],
            [
                new UserRequestDTO
                {
                    PaginationType = PaginationType.Cursor,
                    CursorPagination = new CursorPaginationRequest { PageSize = 100, Cursor = 0, IsQueryPreviousPage = false }
                },
                new CursorPaginationResponse<User>
                {
                    Data = new List<User>(),
                    TotalCount = 0,
                    NextCursor = 100,
                    PreviousCursor = 0
                }
            ]
        ];

        #endregion


        #region GetPaginatedUsers

        [Theory]
        [MemberData(nameof(PaginationTestData))]
        public async Task GetPaginatedUsers_ValidRequest_ReturnsOkWithResult(UserRequestDTO userRequestDTO, UserResponseDTO<User> expectedResultDTO)
        {
            // Arrange
            var request = userRequestDTO;

            var expectedResult = expectedResultDTO;
            
            _mockUserServices
                .Setup(s => s.GetPaginatedListAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetPaginatedUsers(request);

            // Assert
            _mockUserServices.Verify(s => s.GetPaginatedListAsync(request), Times.Once);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public async Task GetPaginatedUsers_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var request = new UserRequestDTO();
            _controller.ModelState.AddModelError("PageSize", "PageSize is required.");

            // Act
            var result = await _controller.GetPaginatedUsers(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        #endregion
    }
}
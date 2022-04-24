using Moq;
using System;
using System.Threading.Tasks;
using TodoManager.Data;
using TodoManager.Data.Entities;
using TodoManager.Data.Services;
using TodoManager.Shared.Entities;
using TodoManager.Shared.Services;
using Xunit;

namespace TodoManager.Api.Tests
{
    public class TodoServiceTests
    {
        private readonly TodoService _todoService;
        private readonly Mock<ITodoRepositoryManager> _mockRepositoryManager = new Mock<ITodoRepositoryManager>();
        private readonly Mock<IUtilityService> _mockUtilityService = new Mock<IUtilityService>();

        public TodoServiceTests()
        {
            _todoService = new TodoService(_mockRepositoryManager.Object, _mockUtilityService.Object);
        }

        [Fact]
        public async Task GetByIdAsync_WhenTodoExists_ShouldReturnTodo()
        {
            // Arrage
            var todoId = 5;
            var creatorId = 2;
            var description = "Testing todo";

            var loggedInUser = new ApplicationUser() { Id = creatorId };

            var todoEntity = new Todo()
            {
                Id = todoId,
                Description = description,
                Deadline = null,
                IsDone = false,
                CreatorId = creatorId,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            _mockUtilityService.Setup(x => x.GetLoggedInUserAsync()).ReturnsAsync(loggedInUser);

            _mockRepositoryManager.Setup(x => x.Todos.GetByIdAsync(creatorId, todoId))
                .ReturnsAsync(todoEntity);

            // Act
            var result = await _todoService.GetByIdAsync(todoId);

            // Assert
            Assert.Equal(todoId, result.Id);
            Assert.Equal(description, result.Description);
        }

        [Fact]
        public async Task GetByIdAsync_WhenTodoDoesNotExists_ThrowsException()
        {
            // Arrage
            var todoId = 5;
            var creatorId = 2;
            var description = "Testing todo";

            var loggedInUser = new ApplicationUser() { Id = creatorId };

            var todoEntity = new Todo()
            {
                Id = todoId,
                Description = description,
                Deadline = null,
                IsDone = false,
                CreatorId = creatorId,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            _mockUtilityService.Setup(x => x.GetLoggedInUserAsync()).ReturnsAsync(loggedInUser);

            _mockRepositoryManager.Setup(x => x.Todos.GetByIdAsync(creatorId, todoId))
                .ReturnsAsync(() => null);

            // Act
            var action = async () => await _todoService.GetByIdAsync(todoId);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(action);
            Assert.Equal("Todo doesn't exists", exception.Message);
        }
    }
}
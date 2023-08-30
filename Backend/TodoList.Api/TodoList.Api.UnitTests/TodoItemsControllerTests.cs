using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Api.DomainObjects;
using TodoList.Api.UnitTests.Builders;
using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System;

namespace TodoList.Api.UnitTests
{
    public class TodoItemsControllerTests
    {
        [Fact]
        public async Task GetTodoItems_ReturnsCorrectResponse()
        {
            //Arrange
            var testList = new List<TodoItem>()
            {
                new TodoItemBuilder().Build(),
                new TodoItemBuilder().Build()
            };

            TodoItemsController controller = new TestTodoItemControllerBuilder()
                .WithGetTodoItemsAsyncReturns(testList)
                .Build();

            //Act
            var response = await controller.GetTodoItems();

            //Assert
            var result = Assert.IsType<OkObjectResult>(response);
            var listResult = result.Value as List<TodoItem>;
            Assert.Equal(2, listResult.Count);
            Assert.Equal(testList[0].Description, listResult[0].Description);
        }

        [Fact]
        public async Task PostTodoItem_WithCorrectModel_ReturnsCorrectResponse()
        {
            //Arrange
            var testTodoItem = new TodoItemBuilder().Build();

            TodoItemsController controller = new TestTodoItemControllerBuilder()
                .WithInsertToDoItemAsyncReturns(testTodoItem)
                .Build();

            //Act
            var response = await controller.PostTodoItem(testTodoItem);

            //Assert
            var result = Assert.IsType<CreatedAtActionResult>(response);
            var todoItemResult = result.Value as TodoItem;
            Assert.Equal(testTodoItem.Id, todoItemResult.Id);
            Assert.Equal(testTodoItem.Description, todoItemResult.Description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task PostTodoItem_WithGivenDescription_ReturnsBadRequest(string description)
        {
            //Arrange
            string expectedMessage = "Description is required.";
            var testTodoItem = new TodoItemBuilder()
                .WithDescription(description)
                .Build();
            var mockLogger = new Mock<ILogger<TodoItemsController>>();

            TodoItemsController controller = new TestTodoItemControllerBuilder()
                .WithMockLogger(mockLogger).Build();

            //Act
            var response = await controller.PostTodoItem(testTodoItem);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(expectedMessage, badRequestResult.Value);
            VerifyLog(expectedMessage, mockLogger);
        }

        [Fact]
        public async Task PostTodoItem_WithItemDescriptionExists_ReturnsBadRequest()
        {
            //Arrange
            string expectedMessage = "Description already exists.";
            var testTodoItem = new TodoItemBuilder().Build();
            var mockLogger = new Mock<ILogger<TodoItemsController>>();

            TodoItemsController controller = new TestTodoItemControllerBuilder()
                .WithTodoItemDescriptionExistsAsyncReturns(true)
                .WithMockLogger(mockLogger)
                .Build();

            //Act
            var response = await controller.PostTodoItem(testTodoItem);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(expectedMessage, badRequestResult.Value);
            VerifyLog(expectedMessage, mockLogger);
        }

        //TODO PutTodoItem

        private static void VerifyLog(string expectedMessage, Mock<ILogger<TodoItemsController>> mockLogger)
        {
            mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((o, t) => string.Equals(expectedMessage, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}

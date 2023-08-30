using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using TodoList.Api.Controllers;
using TodoList.Api.DomainObjects;
using TodoList.Api.Services;

namespace TodoList.Api.UnitTests.Builders
{
    internal class TestTodoItemControllerBuilder
    {
        private readonly Mock<IToDoItemsService> _mockTodoItemsService;
        private Mock<ILogger<TodoItemsController>> _mockLogger;

        internal TestTodoItemControllerBuilder()
        {
            _mockTodoItemsService = new();
            _mockLogger = new();
        }

        internal TestTodoItemControllerBuilder WithMockLogger(Mock<ILogger<TodoItemsController>> mockLogger)
        {
            _mockLogger = mockLogger;
            return this;
        }

        internal TestTodoItemControllerBuilder WithInsertToDoItemAsyncReturns(TodoItem item)
        {
            _mockTodoItemsService
                    .Setup(m => m.InsertToDoItemAsync(It.IsAny<TodoItem>()))
                    .ReturnsAsync(item);
            return this;
        }

        internal TestTodoItemControllerBuilder WithTodoItemDescriptionExistsAsyncReturns(bool v)
        {
            _mockTodoItemsService
                    .Setup(m => m.TodoItemDescriptionExistsAsync(It.IsAny<string>()))
                    .ReturnsAsync(v);
            return this;
        }

        internal TestTodoItemControllerBuilder WithGetTodoItemsAsyncReturns(List<TodoItem> testList)
        {
            _mockTodoItemsService
                    .Setup(m => m.GetTodoItemsAsync())
                    .ReturnsAsync(testList);
            return this;
        }

        internal TodoItemsController Build()
        {
            return new TodoItemsController(_mockTodoItemsService.Object, _mockLogger.Object);
        }
    }
}

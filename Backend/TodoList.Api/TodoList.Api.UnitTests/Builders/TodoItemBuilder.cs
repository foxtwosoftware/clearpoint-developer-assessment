using System;
using TodoList.Api.DomainObjects;

namespace TodoList.Api.UnitTests.Builders
{
    internal class TodoItemBuilder
    {
        private TodoItem _todoItem = new TodoItem();

        public TodoItemBuilder()
        {
            _todoItem.Id = Guid.NewGuid();
            _todoItem.Description = "Default";
        }

        internal TodoItemBuilder WithGuid(Guid guid)
        {
            _todoItem.Id = guid;
            return this;
        }

        internal TodoItemBuilder WithDescription(string description)
        {
            _todoItem.Description = description;
            return this;
        }

        internal TodoItemBuilder WithIsCompleted(bool isCompleted)
        {
            _todoItem.IsCompleted = isCompleted;
            return this;
        }

        internal TodoItem Build()
        {
            return _todoItem;
        }
    }
}

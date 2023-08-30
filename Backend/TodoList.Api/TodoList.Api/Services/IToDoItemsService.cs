using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.DomainObjects;

namespace TodoList.Api.Services
{
    public interface IToDoItemsService
    {
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync();
        Task<TodoItem> GetToDoItemAsync(Guid id);
        Task UpdateToDoItemAsync(TodoItem todoItem);
        Task<TodoItem> InsertToDoItemAsync(TodoItem todoItem);
        Task<bool> TodoItemDescriptionExistsAsync(string description);
    }
}

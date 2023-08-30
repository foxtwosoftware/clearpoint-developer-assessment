using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.DataAccess;
using TodoList.Api.DomainObjects;
using TodoList.Api.Exceptions;

namespace TodoList.Api.Services
{
    public class ToDoItemsService : IToDoItemsService
    {
        private readonly IRepository<TodoItem> _todoItemRepository;
        private readonly ILogger<ToDoItemsService> _logger;

        public ToDoItemsService(IRepository<TodoItem> todoItemRepository, ILogger<ToDoItemsService> logger)
        {
            _todoItemRepository = todoItemRepository;
            _logger = logger;
        }

        public async Task<TodoItem> GetToDoItemAsync(Guid id)
        {
            return await _todoItemRepository.GetAsync(id);
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
        {
            //Ignore completed todo items
            return await _todoItemRepository.FindAsync(x => !x.IsCompleted);
        }

        public async Task UpdateToDoItemAsync(TodoItem todoItem)
        {
            try
            {
                await _todoItemRepository.UpdateAsync(todoItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await TodoItemIdExistsAsync(todoItem.Id))
                {
                    throw;
                }
                else
                {
                    //Todo item already exists so throw custom exception
                    throw new DoesNotExistException();
                }
            }
        }

        public async Task<TodoItem> InsertToDoItemAsync(TodoItem todoItem)
        {
            return await _todoItemRepository.AddAsync(todoItem);
        }

        public async Task<bool> TodoItemDescriptionExistsAsync(string description)
        {
            var results = (await _todoItemRepository
                   .FindAsync(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted))
                   .ToList();

            return results.Count != 0;
        }

        private async Task<bool> TodoItemIdExistsAsync(Guid id)
        {
            var results = (await _todoItemRepository
                .FindAsync(x => x.Id == id))
                .ToList();
            return results.Count != 0;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.DomainObjects;
using TodoList.Api.Exceptions;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IToDoItemsService _toDoItemService;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(IToDoItemsService toDoItemService, ILogger<TodoItemsController> logger)
        {
            _toDoItemService = toDoItemService;
            _logger = logger;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            IEnumerable<TodoItem> results = await _toDoItemService.GetTodoItemsAsync();
            return Ok(results);
        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            TodoItem result = await _toDoItemService.GetToDoItemAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Updates a todo item.  This can be used to mark a to-do item as complete.
        /// </summary>
        // Put: api/TodoItems/...
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return LogAndReturnBadRequest("Id in request does not match object.");
            }

            try
            {
                await _toDoItemService.UpdateToDoItemAsync(todoItem);
            }
            catch (DoesNotExistException)
            {
                return NotFound();
            }

            return NoContent();
        } 

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItem(TodoItem todoItem)
        {
            if (string.IsNullOrEmpty(todoItem?.Description))
            {
                return LogAndReturnBadRequest("Description is required.");
            }
            else if (await _toDoItemService.TodoItemDescriptionExistsAsync(todoItem.Description))
            {
                return LogAndReturnBadRequest("Description already exists.");
            }

            await _toDoItemService.InsertToDoItemAsync(todoItem);
             
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        private IActionResult LogAndReturnBadRequest(string message)
        {
            _logger.LogError(message);
            return BadRequest(message);
        }
    }
}

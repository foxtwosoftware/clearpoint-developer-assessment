using TodoList.Api.DomainObjects;

namespace TodoList.Api.DataAccess
{
    public class TodoItemsRepository : GenericRepository<TodoItem>
    {
        public TodoItemsRepository(TodoContext context) : base(context)
        {
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.DataAccess;
using TodoList.Api.UnitTests.Builders;
using Xunit;

namespace TodoList.Api.UnitTests
{
    public class TodoItemRepositoryTests
    {
        [Fact]
        public async Task FindAsync_WithPredicate_ReturnsCorrectResults()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: "TodoItemsDatabase")
            .Options;

            SeedTestDb(options);

            using (var context = new TodoContext(options))
            {
                //Act
                var todoItemRepo = new TodoItemsRepository(context);
                var result = (await todoItemRepo.FindAsync(x => !x.IsCompleted)).ToList();

                //Assert
                Assert.Equal(2, result.Count);
            }
        }

        //More tests

        private static void SeedTestDb(DbContextOptions<TodoContext> options)
        {
            using (var context = new TodoContext(options))
            {
                context.TodoItems.Add(new TodoItemBuilder().WithIsCompleted(true).Build());
                context.TodoItems.Add(new TodoItemBuilder().Build());
                context.TodoItems.Add(new TodoItemBuilder().Build());
                context.SaveChanges();
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace TodoAPI
{
    public class TodoContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)  { }
    }
}

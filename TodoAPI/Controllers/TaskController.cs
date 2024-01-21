using Microsoft.AspNetCore.Mvc;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly TodoContext _context;
        public TaskController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetTasks")]
        public IEnumerable<Task> Get()
        {
            var tasks = _context.Tasks
                .OrderBy(t => t.Id)
                .ToList();

            return tasks;
        }
    }
}

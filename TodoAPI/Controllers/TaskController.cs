using Microsoft.AspNetCore.Mvc;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("api/todo/")]
    public class TaskController : Controller
    {
        private readonly TodoContext _context;
        public TaskController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("tasks")]
        public IEnumerable<Task> Get()
        {
            var tasks = _context.Tasks
                .OrderBy(t => t.Id)
                .ToList();

            return tasks;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateTask([FromBody] TaskDto task)
        {
            if (task.Title == string.Empty)
            {
                return BadRequest("Title mustn't be empty");
            }
            Task newTask = new Task()
            {
                Title = task.Title,
                Description = task.Description,
                Completed = false
            };
            try
            {
                _context.Tasks.Add(newTask);
                _context.SaveChanges();
            }
            catch (OperationCanceledException)
            {
                BadRequest("Failed adding new entry to the database.");
            }
            catch (Exception)
            {
                return BadRequest("Database error");
            }
            return Ok("Created successfully");
        }

    }
}

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
            // Get all tasks
            var tasks = _context.Tasks
                .OrderBy(t => t.Id)
                .ToList();

            return tasks;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateTask([FromBody] TaskDto task)
        {
            // Title can't be empty
            if (task.Title == string.Empty)
            {
                return BadRequest("Title mustn't be empty");
            }
            // Create new task
            Task newTask = new Task()
            {
                Title = task.Title,
                Description = task.Description,
                Completed = false
            };
            try
            {
                // Update database
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

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteTask([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id value must be greater than 0.");
            }
            // Find task by id
            var task = _context.Tasks
                .FirstOrDefault(t => t.Id == id);

            // Make sure task is not a null
            if (task == null)
            {
                return BadRequest("Id doesn't exit.");
            }
            // Delete the task
            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult UpdateTask([FromRoute] int id, [FromBody] TaskDto update)
        {
            if (id <= 0)
            {
                return BadRequest("Id value must be greater than 0.");
            }
            // Find task by id
            var task = _context.Tasks
                .FirstOrDefault(t => t.Id == id);

            // Make sure task is not a null
            if (task == null)
            {
                return BadRequest("Id doesn't exit.");
            }
            switch (UpdateParts(update))
            {
                case 1:
                    {
                        // Update both title and descr.
                        task.Title = update.Title;
                        task.Description = update.Description;
                        break;
                    }
                case 2:
                    {
                        // Update title only
                        task.Title = update.Title;
                        break;
                    }
                case 3:
                    {
                        // Update description only
                        task.Description = update.Description;
                        break;
                    }
                default:
                    {
                        return BadRequest("Update's content mustn't be empty.");
                    }
            }
            // Make database changes 
            _context.Update(task);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPatch]
        [Route("complete/{id}")]
        public IActionResult CompleteTask([FromRoute] int id, [FromBody] TaskCompleteDto update)
        {
            if (id <= 0)
            {
                return BadRequest("Id value must be greater than 0.");
            }
            // Find task by id
            var task = _context.Tasks
                .FirstOrDefault(t => t.Id == id);

            // Make sure task is not a null
            if (task == null)
            {
                return BadRequest("Id doesn't exit.");
            }
            // Update task completion status
            task.Completed = update.Completed;

            // Make database changes 
            _context.Update(task);
            _context.SaveChanges();

            return Ok();
        }

        private int UpdateParts(TaskDto Dto)
        {
            if (Dto.Title != string.Empty && Dto.Description != string.Empty)
            {
                return 1;
            }
            else if (Dto.Title != string.Empty)
            {
                return 2;
            }
            else if (Dto.Description != string.Empty)
            {
                return 3;
            }
            return 0;
        }
    }
}

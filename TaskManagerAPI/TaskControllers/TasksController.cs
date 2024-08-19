using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApi.Services;
using Task = TaskManagementApi.Data.Task;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<Task>> CreateTask([FromBody] Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTask = await _taskService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.TaskId }, createdTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Task task)
        {
            if (id != task.TaskId) return BadRequest();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedTask = await _taskService.UpdateTaskAsync(task);
            if (updatedTask == null) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var deleted = await _taskService.DeleteTaskAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}

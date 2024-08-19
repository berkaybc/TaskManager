using System.Collections.Generic;
using System.Threading.Tasks;
using Task = TaskManagementApi.Data.Task;
using TaskManagementApi.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using TaskManagementApi.Hubs;

namespace TaskManagementApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger, IHubContext<NotificationHub> hubContext)
        {
            _taskRepository = taskRepository;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<Task>> GetAllTasksAsync()
        {
            _logger.LogInformation("Fetching all tasks from the database.");
            var tasks = await _taskRepository.GetAllTasksAsync();
            _logger.LogInformation($"Retrieved {tasks.Count()} tasks from the database.");
            return tasks;
        }

        public async Task<Task?> GetTaskByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching task from the database with ID: {id}");
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null)
            {
                _logger.LogWarning($"Task with ID: {id} was not found.");
            }
            else
            {
                _logger.LogInformation($"Task with ID: {id} retrieved successfully.");
            }
            return task;
        }

        public async Task<Task> CreateTaskAsync(Task task)
        {
            _logger.LogInformation("Creating a new task.");
            var createdTask = await _taskRepository.CreateTaskAsync(task);
            _logger.LogInformation($"Task with ID: {createdTask.TaskId} created successfully.");

            // Notify clients about the new task
            try
            {
                await _hubContext.Clients.All.SendAsync("TaskCreated", createdTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending the TaskCreated notification.");
            }

            return createdTask;
        }

        public async Task<Task?> UpdateTaskAsync(Task task)
        {
            _logger.LogInformation($"Updating task with ID: {task.TaskId}");
            var updatedTask = await _taskRepository.UpdateTaskAsync(task);
            if (updatedTask == null)
            {
                _logger.LogWarning($"Task with ID: {task.TaskId} could not be updated because it does not exist.");
            }
            else
            {
                _logger.LogInformation($"Task with ID: {task.TaskId} updated successfully.");

                // Notify clients about the task update
                try
                {
                    await _hubContext.Clients.All.SendAsync("TaskUpdated", updatedTask);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while sending the TaskUpdated notification.");
                }
            }
            return updatedTask;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            _logger.LogInformation($"Deleting task with ID: {id}");
            var result = await _taskRepository.DeleteTaskAsync(id);
            if (result)
            {
                _logger.LogInformation($"Task with ID: {id} deleted successfully.");

                // Notify clients about the task deletion
                try
                {
                    await _hubContext.Clients.All.SendAsync("TaskDeleted", id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while sending the TaskDeleted notification.");
                }
            }
            else
            {
                _logger.LogWarning($"Task with ID: {id} could not be deleted because it does not exist.");
            }
            return result;
        }
    }
}

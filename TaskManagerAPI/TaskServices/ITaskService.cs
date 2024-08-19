using System.Collections.Generic;
using System.Threading.Tasks;
using Task = TaskManagementApi.Data.Task;

namespace TaskManagementApi.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Task>> GetAllTasksAsync();
        Task<Task?> GetTaskByIdAsync(int id);
        Task<Task> CreateTaskAsync(Task task);
        Task<Task?> UpdateTaskAsync(Task task);
        Task<bool> DeleteTaskAsync(int id);
    }
}

using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagementApi.Controllers;
using TaskManagementApi.Services;

public class TasksControllerTests
{
    [Fact]
    public async void GetAllTasks_ReturnsOkObjectResult_WithTasks()
    {
        // Arrange
        var mockService = new Mock<ITaskService>();
        mockService.Setup(s => s.GetAllTasksAsync()).ReturnsAsync(new List<TaskManagementApi.Data.Task> { new TaskManagementApi.Data.Task { Title = "Test Task" } });
        var controller = new TasksController(mockService.Object);

        // Act
        var result = await controller.GetAllTasks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<TaskManagementApi.Data.Task>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async void GetTaskById_ReturnsTask_WhenTaskExists()
    {
        // Arrange
        var mockService = new Mock<ITaskService>();
        var taskId = 1;
        mockService.Setup(s => s.GetTaskByIdAsync(taskId)).ReturnsAsync(new TaskManagementApi.Data.Task { TaskId = taskId, Title = "Existing Task" });
        var controller = new TasksController(mockService.Object);

        // Act
        var result = await controller.GetTaskById(taskId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var task = Assert.IsType<TaskManagementApi.Data.Task>(okResult.Value);
        Assert.Equal(taskId, task.TaskId);
    }

    [Fact]
    public async void CreateTask_ReturnsCreatedAtActionResult_WhenTaskIsCreated()
    {
        // Arrange
        var mockService = new Mock<ITaskService>();
        var newTask = new TaskManagementApi.Data.Task { Title = "New Task" };
        mockService.Setup(s => s.CreateTaskAsync(newTask)).ReturnsAsync(new TaskManagementApi.Data.Task { TaskId = 1, Title = "New Task" });
        var controller = new TasksController(mockService.Object);

        // Act
        var result = await controller.CreateTask(newTask);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var task = Assert.IsType<TaskManagementApi.Data.Task>(createdAtActionResult.Value);
        Assert.Equal("New Task", task.Title);
    }

    [Fact]
    public async void UpdateTask_ReturnsNoContentResult_WhenTaskIsUpdated()
    {
        // Arrange
        var mockService = new Mock<ITaskService>();
        var updatedTask = new TaskManagementApi.Data.Task { TaskId = 1, Title = "Updated Task" };
        mockService.Setup(s => s.UpdateTaskAsync(updatedTask)).ReturnsAsync(updatedTask);
        var controller = new TasksController(mockService.Object);

        // Act
        var result = await controller.UpdateTask(updatedTask.TaskId, updatedTask);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async void DeleteTask_ReturnsNoContentResult_WhenTaskIsDeleted()
    {
        // Arrange
        var mockService = new Mock<ITaskService>();
        var taskId = 1;
        mockService.Setup(s => s.DeleteTaskAsync(taskId)).ReturnsAsync(true);
        var controller = new TasksController(mockService.Object);

        // Act
        var result = await controller.DeleteTask(taskId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}

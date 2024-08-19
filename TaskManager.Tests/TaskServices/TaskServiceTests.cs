using Moq;
using TaskManagementApi.Data;
using TaskManagementApi.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TaskManagementApi.Hubs;
public class TaskServiceTests
{
    [Fact]
    public async void GetAllTasksAsync_CallsRepository_ReturnsTasks()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        mockRepo.Setup(repo => repo.GetAllTasksAsync()).ReturnsAsync(new List<TaskManagementApi.Data.Task> { new TaskManagementApi.Data.Task { Title = "Test" } });
        var service = new TaskService(mockRepo.Object, Mock.Of<ILogger<TaskService>>(), Mock.Of<IHubContext<NotificationHub>>());

        // Act
        var tasks = await service.GetAllTasksAsync();

        // Assert
        Assert.NotNull(tasks);
        Assert.Single(tasks);
        mockRepo.Verify(repo => repo.GetAllTasksAsync(), Times.Once);
    }

    [Fact]
    public async void GetTaskByIdAsync_ReturnsTask()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        int taskId = 1;
        mockRepo.Setup(repo => repo.GetTaskByIdAsync(taskId)).ReturnsAsync(new TaskManagementApi.Data.Task { TaskId = taskId, Title = "Test" });
        var service = new TaskService(mockRepo.Object, Mock.Of<ILogger<TaskService>>(), Mock.Of<IHubContext<NotificationHub>>());

        // Act
        var task = await service.GetTaskByIdAsync(taskId);

        // Assert
        Assert.NotNull(task);
        Assert.Equal(taskId, task.TaskId);
        mockRepo.Verify(repo => repo.GetTaskByIdAsync(taskId), Times.Once);
    }

    [Fact]
    public async void CreateTaskAsync_CreatesAndReturnsTask()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        var mockHubContext = new Mock<IHubContext<NotificationHub>>();
        var newTask = new TaskManagementApi.Data.Task { Title = "New Task" };
        mockRepo.Setup(repo => repo.CreateTaskAsync(newTask)).ReturnsAsync(new TaskManagementApi.Data.Task { TaskId = 1, Title = "New Task" });
        var service = new TaskService(mockRepo.Object, Mock.Of<ILogger<TaskService>>(), mockHubContext.Object);

        // Act
        var task = await service.CreateTaskAsync(newTask);

        // Assert
        Assert.NotNull(task);
        Assert.Equal("New Task", task.Title);
        mockRepo.Verify(repo => repo.CreateTaskAsync(newTask), Times.Once);
        //mockHubContext.Verify(hub => hub.Clients.All.SendCoreAsync("ReceiveMessage", It.IsAny<object[]>(), default), Times.Once);
    }

    [Fact]
    public async void UpdateTaskAsync_UpdatesAndReturnsTask()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        var updatedTask = new TaskManagementApi.Data.Task { TaskId = 1, Title = "Updated Task" };
        mockRepo.Setup(repo => repo.UpdateTaskAsync(updatedTask)).ReturnsAsync(updatedTask);
        var service = new TaskService(mockRepo.Object, Mock.Of<ILogger<TaskService>>(), Mock.Of<IHubContext<NotificationHub>>());

        // Act
        var result = await service.UpdateTaskAsync(updatedTask);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Task", result.Title);
        mockRepo.Verify(repo => repo.UpdateTaskAsync(updatedTask), Times.Once);
    }

    [Fact]
    public async void DeleteTaskAsync_DeletesTask_ReturnsTrue()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        int taskId = 1;
        mockRepo.Setup(repo => repo.DeleteTaskAsync(taskId)).ReturnsAsync(true);
        var service = new TaskService(mockRepo.Object, Mock.Of<ILogger<TaskService>>(), Mock.Of<IHubContext<NotificationHub>>());

        // Act
        var result = await service.DeleteTaskAsync(taskId);

        // Assert
        Assert.True(result);
        mockRepo.Verify(repo => repo.DeleteTaskAsync(taskId), Times.Once);
    }
}

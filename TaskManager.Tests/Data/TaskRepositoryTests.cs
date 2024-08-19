using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
public class TaskRepositoryTests
{
    private DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: "TaskDbTest")
        .Options;

    [Fact]
    public async void GetAllTasksAsync_ReturnsTasks()
    {
        // Arrange
        using var context = new AppDbContext(dbContextOptions);
        var repository = new TaskRepository(context);
        context.Tasks.Add(new TaskManagementApi.Data.Task { Title = "Test Task 1" });
        context.SaveChanges();

        // Act
        var tasks = await repository.GetAllTasksAsync();

        // Assert
        Assert.Single(tasks);
    }

    [Fact]
    public async void GetTaskByIdAsync_ReturnsTask()
    {
        // Arrange
        using var context = new AppDbContext(dbContextOptions);
        var repository = new TaskRepository(context);
        var task = new TaskManagementApi.Data.Task { Title = "Test Task 2" };
        context.Tasks.Add(task);
        context.SaveChanges();

        // Act
        var fetchedTask = await repository.GetTaskByIdAsync(task.TaskId);

        // Assert
        Assert.NotNull(fetchedTask);
        Assert.Equal("Test Task 2", fetchedTask.Title);
    }

    [Fact]
    public async void CreateTaskAsync_CreatesTaskSuccessfully()
    {
        // Arrange
        using var context = new AppDbContext(dbContextOptions);
        var repository = new TaskRepository(context);
        var task = new TaskManagementApi.Data.Task { Title = "New Task" };

        // Act
        var createdTask = await repository.CreateTaskAsync(task);

        // Assert
        Assert.NotNull(createdTask);
        Assert.Equal("New Task", createdTask.Title);
        Assert.True(createdTask.TaskId > 0);
    }

    [Fact]
    public async void UpdateTaskAsync_UpdatesTaskSuccessfully()
    {
        // Arrange
        using var context = new AppDbContext(dbContextOptions);
        var repository = new TaskRepository(context);
        var task = new TaskManagementApi.Data.Task { Title = "Old Task" };
        context.Tasks.Add(task);
        context.SaveChanges();

        // Act
        task.Title = "Updated Task";
        var updatedTask = await repository.UpdateTaskAsync(task);

        // Assert
        Assert.NotNull(updatedTask);
        Assert.Equal("Updated Task", updatedTask.Title);
    }

    [Fact]
    public async void DeleteTaskAsync_DeletesTaskSuccessfully()
    {
        // Arrange
        using var context = new AppDbContext(dbContextOptions);
        var repository = new TaskRepository(context);
        var task = new TaskManagementApi.Data.Task { Title = "Task to Delete" };
        context.Tasks.Add(task);
        context.SaveChanges();

        // Act
        var result = await repository.DeleteTaskAsync(task.TaskId);

        // Assert
        Assert.True(result);
        var deletedTask = await repository.GetTaskByIdAsync(task.TaskId);
        Assert.Null(deletedTask);
    }
}

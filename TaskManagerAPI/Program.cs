using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Hubs;
using TaskManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


// Add services to the container.
// Register DbContext with In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TaskDb"));

// Register repositories and services
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Add Controllers
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // URL of your React app
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();
app.UseCors("AllowFrontend");
app.UseRouting();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<TaskManagementApi.Middleware.ExceptionHandlingMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Map controllers to handle API requests
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

using TaskBoard.API.DTOs;
using TaskBoard.API.Models;
using TaskBoard.API.Repositories;

namespace TaskBoard.API.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _tasks;
    public TaskService(ITaskRepository tasks) => _tasks = tasks;

    public async Task<List<TaskResponse>> GetAllAsync(int userId)
    {
        var items = await _tasks.GetAllByUserAsync(userId);
        return items.Select(Map).ToList();
    }

    public async Task<TaskResponse> CreateAsync(int userId, CreateTaskRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Title))
            throw new ArgumentException("Title is required.");

        var task = new TaskItem
        {
            Title = req.Title.Trim(),
            Description = req.Description?.Trim() ?? string.Empty,
            Status = "Pending",
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        return Map(await _tasks.AddAsync(task));
    }

    public async Task<TaskResponse> UpdateAsync(int userId, int taskId, UpdateTaskRequest req)
    {
        var task = await GetOwnedOrThrowAsync(taskId, userId);
        task.Title = req.Title.Trim();
        task.Description = req.Description?.Trim() ?? string.Empty;
        return Map(await _tasks.UpdateAsync(task));
    }

    public async Task<TaskResponse> CompleteAsync(int userId, int taskId)
    {
        var task = await GetOwnedOrThrowAsync(taskId, userId);
        task.Status = "Completed";
        task.CompletedAt = DateTime.UtcNow;
        return Map(await _tasks.UpdateAsync(task));
    }

    public async Task DeleteAsync(int userId, int taskId)
    {
        var task = await GetOwnedOrThrowAsync(taskId, userId);
        await _tasks.DeleteAsync(task);
    }

    private async Task<TaskItem> GetOwnedOrThrowAsync(int taskId, int userId)
    {
        var task = await _tasks.GetByIdAsync(taskId, userId)
                   ?? throw new KeyNotFoundException("Task not found.");
        return task;
    }

    private static TaskResponse Map(TaskItem t) =>
        new(t.Id, t.Title, t.Description, t.Status, t.CreatedAt, t.CompletedAt);
}
using Microsoft.EntityFrameworkCore;
using TaskBoard.API.Data;
using TaskBoard.API.Models;

namespace TaskBoard.API.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;
    public TaskRepository(AppDbContext db) => _db = db;

    public Task<List<TaskItem>> GetAllByUserAsync(int userId) =>
        _db.Tasks
           .Where(t => t.UserId == userId)
           .OrderByDescending(t => t.CreatedAt)
           .ToListAsync();

    public Task<TaskItem?> GetByIdAsync(int id, int userId) =>
        _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem> UpdateAsync(TaskItem task)
    {
        _db.Tasks.Update(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task DeleteAsync(TaskItem task)
    {
        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
    }
}
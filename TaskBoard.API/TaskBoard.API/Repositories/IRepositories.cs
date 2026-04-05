using TaskBoard.API.Models;

namespace TaskBoard.API.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<User> AddAsync(User user);
}

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllByUserAsync(int userId);
    Task<TaskItem?> GetByIdAsync(int id, int userId);
    Task<TaskItem> AddAsync(TaskItem task);
    Task<TaskItem> UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
}
using TaskBoard.API.DTOs;

namespace TaskBoard.API.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest req);
    Task<AuthResponse> LoginAsync(LoginRequest req);
}

public interface ITaskService
{
    Task<List<TaskResponse>> GetAllAsync(int userId);
    Task<TaskResponse> CreateAsync(int userId, CreateTaskRequest req);
    Task<TaskResponse> UpdateAsync(int userId, int taskId, UpdateTaskRequest req);
    Task<TaskResponse> CompleteAsync(int userId, int taskId);
    Task DeleteAsync(int userId, int taskId);
}
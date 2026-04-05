namespace TaskBoard.API.DTOs;

//  Auth
public record RegisterRequest(string Name, string Email, string Password);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, string Name, string Email);

//  Task
public record CreateTaskRequest(string Title, string Description);
public record UpdateTaskRequest(string Title, string Description);

public record TaskResponse(
    int Id,
    string Title,
    string Description,
    string Status,
    DateTime CreatedAt,
    DateTime? CompletedAt
);
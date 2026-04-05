using Microsoft.EntityFrameworkCore;
using TaskBoard.API.Data;
using TaskBoard.API.Models;

namespace TaskBoard.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public Task<User?> GetByEmailAsync(string email) =>
        _db.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());

    public Task<bool> EmailExistsAsync(string email) =>
        _db.Users.AnyAsync(u => u.Email == email.ToLower());

    public async Task<User> AddAsync(User user)
    {
        
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
   
}
using Microsoft.EntityFrameworkCore;
using TaskBoard.API.Models;

namespace TaskBoard.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Email).IsRequired().HasMaxLength(256);
                e.Property(u => u.Name).IsRequired().HasMaxLength(100);
                e.Property(u => u.PasswordHash).IsRequired();
            });

            b.Entity<TaskItem>(e =>
            {
                e.HasKey(t => t.Id);
                e.Property(t => t.Title).IsRequired().HasMaxLength(200);
                e.Property(t => t.Description).HasMaxLength(1000);
                e.Property(t => t.Status).IsRequired().HasMaxLength(20);
                e.HasOne(t => t.User)
                 .WithMany(u => u.Tasks)
                 .HasForeignKey(t => t.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
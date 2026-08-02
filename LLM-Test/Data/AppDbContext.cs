using LLM_Test.Data.Entities;
using Microsoft.EntityFrameworkCore;

using Thread = LLM_Test.Data.Entities.Thread;

namespace LLM_Test.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Thread> Threads { get; set; }
    public DbSet<Message> messages { get; set; }

    public DbSet<ImageAttached> ImageAttacheds { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Gmail)
            .IsUnique();

        modelBuilder.Entity<Thread>()
            .HasOne(t => t.User)
            .WithMany(u => u.Threads)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Message>()
            .HasOne(u => u.Thread)
            .WithMany(u => u.Messages)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ImageAttached>()
            .HasOne(u => u.Message)
            .WithMany(m => m.ImageAttacheds)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

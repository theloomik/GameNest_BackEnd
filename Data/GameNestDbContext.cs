using GameNest_BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace GameNest_BackEnd.Data;

public class GameNestDbContext : DbContext
{
    public GameNestDbContext(DbContextOptions<GameNestDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Унікальний Username
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // Унікальний Email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // UserGame → User
        modelBuilder.Entity<UserGame>()
            .HasOne(ug => ug.User)
            .WithMany(u => u.UserGames)
            .HasForeignKey(ug => ug.UserId);

        // UserGame → Game
        modelBuilder.Entity<UserGame>()
            .HasOne(ug => ug.Game)
            .WithMany(g => g.UserGames)
            .HasForeignKey(ug => ug.GameId);

        // GameSubmission → User (хто подав)
        modelBuilder.Entity<GameSubmission>()
            .HasOne(gs => gs.SubmittedByUser)
            .WithMany(u => u.GameSubmissions)
            .HasForeignKey(gs => gs.SubmittedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // GameSubmission → Game
        modelBuilder.Entity<GameSubmission>()
            .HasOne(gs => gs.Game)
            .WithMany(g => g.GameSubmissions)
            .HasForeignKey(gs => gs.GameId)
            .OnDelete(DeleteBehavior.SetNull);

        // GameSubmission → User (хто перевірив)
        modelBuilder.Entity<GameSubmission>()
            .HasOne(gs => gs.ReviewedByUser)
            .WithMany()
            .HasForeignKey(gs => gs.ReviewedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Game> Games { get; set; }

    public DbSet<UserGame> UserGames { get; set; }

    public DbSet<GameSubmission> GameSubmissions { get; set; }
}
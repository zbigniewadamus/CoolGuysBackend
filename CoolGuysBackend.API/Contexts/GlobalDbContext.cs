using System.Reflection;
using CoolGuysBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoolGuysBackend.Contexts;

public class GlobalDbContext: DbContext
{
    public GlobalDbContext(DbContextOptions<GlobalDbContext> options) : base(options)
    {
    }
    protected GlobalDbContext() {}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        modelBuilder.Entity<FriendEntity>()
            .HasOne(f => f.User1)
            .WithMany()
            .HasForeignKey(f => f.User1Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FriendEntity>()
            .HasOne(f => f.User2)
            .WithMany()
            .HasForeignKey(f => f.User2Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserDataEntity>()
            .HasOne(ud => ud.User)
            .WithOne(u => u.UserData)
            .HasForeignKey<UserEntity>(u => u.UserDataId);
            

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<UserDataEntity> UserDataEntities { get; set; }
    public DbSet<UserEntity> UserEntities { get; set; }
    public DbSet<FriendEntity> FriendEntities { get; set; }
    public DbSet<PostEntity> PostEntities { get; set; }
}
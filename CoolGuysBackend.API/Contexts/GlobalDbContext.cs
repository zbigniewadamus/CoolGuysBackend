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
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<UserDataEntity> UserDataEntities { get; set; }
    public DbSet<FriendEntity> FriendEntities { get; set; }
}
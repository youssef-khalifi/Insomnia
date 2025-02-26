using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data;

public class SqliteDbContext : DbContext
{
    public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Collection> Collections { get; set; }
    public DbSet<ResponseApi> Responses { get; set; }
    public DbSet<RequestApi> Requests { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        // Configure SyncId as an alternate key
        modelBuilder.Entity<Collection>()
            .HasIndex(c => c.SyncId)
            .IsUnique();
        modelBuilder.Entity<ResponseApi>()
            .HasIndex(resp => resp.SyncId)
            .IsUnique();
        modelBuilder.Entity<RequestApi>()
            .HasIndex(req => req.SyncId)
            .IsUnique();
        
        modelBuilder.Entity<Collection>()
            .HasMany(c => c.Requests)
            .WithOne(r => r.Collection)
            .HasForeignKey(r => r.CollectionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        
        // Soft delete query filter
        modelBuilder.Entity<Collection>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<RequestApi>().HasQueryFilter(req => !req.IsDeleted);
        modelBuilder.Entity<ResponseApi>().HasQueryFilter(resp => !resp.IsDeleted);
    }
}
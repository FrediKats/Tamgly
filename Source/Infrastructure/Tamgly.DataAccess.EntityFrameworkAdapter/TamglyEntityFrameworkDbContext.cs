using Tamgly.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Tamgly.DataAccess.EntityFrameworkAdapter;

public class TamglyEntityFrameworkDbContext : DbContext
{
    public DbSet<WorkItemDatabaseRecord> WorkItems { get; set; }
    public DbSet<WorkItemTrackIntervalDatabaseRecord> WorkItemTrackIntervals { get; set; }
    public DbSet<ProjectDatabaseRecord> Projects { get; set; }
    public DbSet<ProjectWorkItemDatabaseRecord> ProjectWorkItems { get; set; }
    public DbSet<RepetitiveWorkItemConfigurationDatabaseRecord> RepetitiveWorkItemConfigurations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("in-memory-ef");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder
        //    .Entity<ProjectWorkItemDatabaseRecord>()
        //    .HasKey(v => new {v.ProjectId, v.WorkItemId});

        base.OnModelCreating(modelBuilder);
    }
}
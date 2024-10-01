using Mats.Edvardsen.TestingStuff.Data.AuditFeature;
using Mats.Edvardsen.TestingStuff.Data.UserFeature;
using Microsoft.EntityFrameworkCore;

namespace Mats.Edvardsen.TestingStuff.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<EntityAudit> Audits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityAudit>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<EntityAudit>()
            .Property(b => b.Id)
            .IsRequired();
        modelBuilder.Entity<EntityAudit>()
            .Property(b => b.EntityId)
            .IsRequired();
        modelBuilder.Entity<EntityAudit>()
            .Property(b => b.TimeStamp)
            .IsRequired();
        modelBuilder.Entity<EntityAudit>()
            .Property(b => b.EntityState)
            .IsRequired();

        modelBuilder.Entity<UserEntity>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<UserEntity>()
            .Property(b => b.Id)
            .IsRequired();
        modelBuilder.Entity<UserEntity>()
            .Property(b => b.Name)
            .IsRequired();
        modelBuilder.Entity<UserEntity>()
            .Property(b => b.Age)
            .IsRequired();
    }
}
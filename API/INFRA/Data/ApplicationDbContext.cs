using PATOA.CORE.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Expressions;

namespace PATOA.INFRA.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    public DbSet<PrivatePat> PrivatePats { get; set; }

    public DbSet<PublicPat> PublicPats { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<EnginType> EnginTypes { get; set; }

    public DbSet<Engin> Engins { get; set; }

    public DbSet<Insurance> Insurances { get; set; }

    public DbSet<Official> Officials { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure base entity properties for all entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                // Configure default values for base entity properties
                modelBuilder.Entity(entityType.ClrType)
                    .Property("CreatedOn")
                    .HasDefaultValueSql("GETUTCDATE()");

                modelBuilder.Entity(entityType.ClrType)
                    .Property("UpdatedOn")
                    .HasDefaultValueSql("GETUTCDATE()");

                modelBuilder.Entity(entityType.ClrType)
                    .Property("IsActive")
                    .HasDefaultValue(true);

                modelBuilder.Entity(entityType.ClrType)
                    .Property("IsDeleted")
                    .HasDefaultValue(false);
            }
        }

        // Account configurations
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasIndex(a => a.Username).IsUnique();
            entity.HasIndex(a => a.Email).IsUnique();
            entity.HasOne(a => a.Role)
                .WithMany(r=>r.Accounts)
                .HasForeignKey(a => a.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

         
        
        // Right configurations
        modelBuilder.Entity<Right>(entity =>
        {
            entity.HasIndex(r => r.Name).IsUnique();
            entity.HasOne(r => r.Role)
                .WithMany()
                .HasForeignKey(r => r.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        // Official configurations
        modelBuilder.Entity<Official>(entity =>
        {
            entity.HasIndex(o => o.CIN).IsUnique();
        });









        // Engin configurations
        modelBuilder.Entity<Engin>(entity =>
        {
            entity.HasIndex(e => e.Matricule).IsUnique();
        });

        // Insurance configurations
        modelBuilder.Entity<Insurance>(entity =>
        {
            entity.HasOne(i => i.Engin)
                .WithMany()
                .HasForeignKey(i => i.EnginId)
                .OnDelete(DeleteBehavior.Restrict);
        });
 

 

        // Soft delete filter
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var propertyAccess = Expression.Property(parameter, "IsDeleted");
                var notExpression = Expression.Not(propertyAccess);
                var lambda = Expression.Lambda(notExpression, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
} 
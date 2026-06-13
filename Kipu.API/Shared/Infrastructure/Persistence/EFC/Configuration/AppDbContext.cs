using Kipu.API.IAM.Domain.Model.Aggregates;
using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context
/// </summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<MaterialInventory> MaterialInventories { get; set; }
    public DbSet<MaterialCatalog> MaterialCatalogs { get; set; }
    public DbSet<MaterialCategory> MaterialCategories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; } 
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>().Property(u => u.Name).IsRequired();
        builder.Entity<User>().Property(u => u.Email).IsRequired();
        builder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
        builder.Entity<User>().Property(u => u.Role).IsRequired();
        builder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        // Project mapping
        builder.Entity<Project>().HasKey(p => p.Id);
        builder.Entity<Project>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Project>().Property(p => p.Name).IsRequired();
        builder.Entity<Project>().Property(p => p.Location).IsRequired();
        builder.Entity<Project>().Property(p => p.Budget).IsRequired();
        builder.Entity<Project>().Property(p => p.Status).IsRequired();
        builder.Entity<Project>().HasIndex(p => p.Name).IsUnique();

        // ProjectItem mapping
        builder.Entity<ProjectItem>().HasKey(pi => pi.Id);
        builder.Entity<ProjectItem>().Property(pi => pi.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ProjectItem>().Property(pi => pi.Name).IsRequired();
        builder.Entity<ProjectItem>().Property(pi => pi.StartDate).IsRequired();
        builder.Entity<ProjectItem>().Property(pi => pi.EndDate).IsRequired();
        builder.Entity<ProjectItem>()
            .HasOne(pi => pi.Project)
            .WithMany(p => p.Items)
            .HasForeignKey(pi => pi.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // MaterialInventory mapping
        builder.Entity<MaterialInventory>(entity =>
        {
            entity.HasKey(mi => mi.Id);
            entity.Property(mi => mi.Id).IsRequired().ValueGeneratedOnAdd();
            
            entity.Property(mi => mi.ProjectId)
                .HasConversion(p => p.Value, v => new ProjectId(v))
                .IsRequired();

            entity.Property(mi => mi.MaterialCatalogId)
                .HasConversion(m => m.Value, v => new MaterialCatalogId(v))
                .IsRequired();

            entity.Property(mi => mi.CurrentStock)
                .HasConversion(q => q.Value, v => new Quantity(v))
                .IsRequired();

            entity.Property(mi => mi.MinimumStock)
                .HasConversion(q => q.Value, v => new Quantity(v))
                .IsRequired();

            entity.Property(mi => mi.Location)
                .HasConversion(l => l.Value, v => new WarehouseLocation(v))
                .IsRequired();
        });
        
        // MaterialCatalog mapping
        builder.Entity<MaterialCatalog>(entity =>
        {
            entity.HasKey(mc => mc.Id);
            entity.Property(mc => mc.Id).IsRequired().ValueGeneratedOnAdd();
            
            entity.Property(mc => mc.Name)
                .HasConversion(n => n.Value, v => new Name(v))
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(mc => mc.CategoryId)
                .HasConversion(c => c.Value, v => new CategoryId(v))
                .IsRequired();

            entity.Property(mc => mc.MeasureUnit)
                .HasConversion<int>()
                .IsRequired();
            
            entity.HasIndex(mc => mc.Name).IsUnique();
        });
        // MaterialCategory mapping
        
        builder.Entity<MaterialCategory>(entity =>
        {
            entity.HasKey(mc => mc.Id);
            entity.Property(mc => mc.Id).IsRequired().ValueGeneratedOnAdd();
            
            entity.Property(mc => mc.Name)
                .HasConversion(n => n.Value, v => new Name(v))
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(mc => mc.Description)
                .HasMaxLength(500);

            entity.Property(mc => mc.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            
            entity.HasIndex(mc => mc.Name).IsUnique();
            
            entity.HasIndex(mc => mc.IsActive);
        });
        // Supplier mapping
        builder.Entity<Supplier>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(s => s.Ruc)
                .HasConversion(r => r.Value, v => new Ruc(v))
                .IsRequired()
                .HasMaxLength(11);

            entity.Property(s => s.SocialReason)
                .HasConversion(r => r.Value, v => new SocialReason(v))
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(s => s.Contact)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.Phone)
                .HasConversion(p => p.Value, v => new Phone(v))
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(s => s.Email)
                .HasConversion(e => e.Value, v => new Email(v))
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.PaymentTerms)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            entity.HasIndex(s => s.Ruc).IsUnique();
            entity.HasIndex(s => s.Email).IsUnique();
            entity.HasIndex(s => s.IsActive);
        });
        builder.UseSnakeCaseNamingConvention();
    }
}
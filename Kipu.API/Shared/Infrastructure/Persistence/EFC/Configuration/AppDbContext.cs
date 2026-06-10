using Kipu.API.IAM.Domain.Model.Aggregates;
using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context
/// </summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
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

        builder.UseSnakeCaseNamingConvention();
    }
}
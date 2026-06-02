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
        /*
         builder.Entity<AggregateName>().HasKey(f => f.Id);
           builder.Entity<AggregateName>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
           builder.Entity<AggregateName>()
               .Property(f => f.SourceId)
               .HasConversion(valueObject => valueObject.Value, value => new ValueObjectName(value))
               .IsRequired();

           builder.Entity<AggregateName>()
               .Property(f => f.NewsApiKey)
               .HasConversion(valueObject => valueObject.Value, value => new ValueObjectName(value))
               .IsRequired();
           builder.Entity<AggregateName>()
               .HasIndex(f => new { f.NewsApiKey, f.SourceId })
               .IsUnique();
         */
        builder.UseSnakeCaseNamingConvention();
    }
}
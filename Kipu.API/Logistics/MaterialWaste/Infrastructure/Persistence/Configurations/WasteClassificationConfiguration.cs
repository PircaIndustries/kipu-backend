using Kipu.API.Logistics.MaterialWaste.Domain.Model.Aggregates;
using Kipu.API.Logistics.MaterialWaste.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kipu.API.Logistics.MaterialWaste.Infrastructure.Persistence.Configurations;

public class WasteClassificationConfiguration : IEntityTypeConfiguration<WasteClassification>
{
    public void Configure(EntityTypeBuilder<WasteClassification> builder)
    {
        builder.ToTable("WasteClassifications");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, value => new WasteClassificationId(value))
            .IsRequired();

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
    }
}
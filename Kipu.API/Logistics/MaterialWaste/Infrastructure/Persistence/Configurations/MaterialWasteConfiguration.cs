using Kipu.API.Logistics.MaterialWaste.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kipu.API.Logistics.MaterialWaste.Infrastructure.Persistence.Configurations;

public class MaterialWasteConfiguration : IEntityTypeConfiguration<Domain.Model.Aggregates.MaterialWaste>
{
    public void Configure(EntityTypeBuilder<Domain.Model.Aggregates.MaterialWaste> builder)
    {
        builder.ToTable("MaterialWastes");
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .HasConversion(id => id.Value, value => new MaterialWasteId(value))
            .IsRequired();

        builder.Property(w => w.ProjectId).IsRequired();
        builder.Property(w => w.MaterialId).IsRequired();
        builder.Property(w => w.Quantity).IsRequired();
        builder.Property(w => w.ClassificationType).IsRequired().HasMaxLength(50);
        builder.Property(w => w.Date).IsRequired().HasMaxLength(10);
        builder.Property(w => w.Description).HasMaxLength(500);
        builder.Property(w => w.ReportedBy).HasMaxLength(100);
        builder.Property(w => w.PhotoUrl).HasMaxLength(2048);
    }
}
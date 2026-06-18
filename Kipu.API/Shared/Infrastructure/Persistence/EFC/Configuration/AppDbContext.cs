using Kipu.API.Document.Domain.Model.ValueObjects;
using Kipu.API.IAM.Domain.Model.Aggregates;
using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Interceptors;
using Kipu.API.Team.TeamUser.domain.model.Aggregates;
using Kipu.API.Team.TeamWorker.Domain.Model.Aggregates;
using Kipu.API.Team.TeamWorker.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using TeamUserId = Kipu.API.Team.TeamUser.domain.model.ValueObjects.UserId;
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
    public DbSet<MaterialRequest> MaterialRequests { get; set; }
    public DbSet<MaterialRequestItem> MaterialRequestItems { get; set; }
    public DbSet<Kipu.API.Budget.Domain.Model.Aggregates.BudgetItem> BudgetItems { get; set; }
    public DbSet<Kipu.API.Progress.Domain.Model.Aggregates.ProgressItem> ProgressItems { get; set; }
    public DbSet<Kipu.API.Budget.Domain.Model.Entities.BudgetTransaction> BudgetTransactions { get; set; }
    public DbSet<SupplierOffer> SupplierOffers { get; set; }
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
        // MaterialRequest mapping
        builder.Entity<MaterialRequest>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(r => r.Deadline)
                .IsRequired();

            entity.Property(r => r.RequestStatus)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(r => r.RequestPriority)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(r => r.DeliveryLocation)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(r => r.BudgetLineId)
                .HasConversion(
                    b => b == null ? (int?)null : b.Value,
                    v => v.HasValue ? new BudgetLineId(v.Value) : null
                );

            entity.Property(r => r.Purpose)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(r => r.AdditionalNotes)
                .HasMaxLength(2000);

            entity.Property(r => r.RequestedBy)
                .HasConversion(u => u.Value, v => new UserId(v))
                .IsRequired();

            entity.HasIndex(r => r.RequestStatus);
        });
        // MaterialRequestItem mapping
        builder.Entity<MaterialRequestItem>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(i => i.MaterialCatalogId)
                .HasConversion(m => m.Value, v => new MaterialCatalogId(v))
                .IsRequired();

            entity.Property(i => i.SupplierId)
                .HasConversion(s => s.Value, v => new SupplierId(v))
                .IsRequired();

            entity.Property(i => i.Quantity)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(i => i.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.HasOne<MaterialRequest>()
                .WithMany(r => r.Items)
                .HasForeignKey(i => i.MaterialRequestId)
                .OnDelete(DeleteBehavior.Cascade);
            
        });
        builder.UseSnakeCaseNamingConvention();
        
        // SupplierOffer mapping
        builder.Entity<SupplierOffer>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();

            entity.Property(o => o.SupplierId)
                .HasConversion(s => s.Value, v => new SupplierId(v))
                .IsRequired();

            entity.Property(o => o.MaterialCatalogId)
                .HasConversion(m => m.Value, v => new MaterialCatalogId(v))
                .IsRequired();

            entity.Property(o => o.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.HasIndex(o => new { o.SupplierId, o.MaterialCatalogId }).IsUnique();
        });
        // TEAM USERS ---
        
        builder.Entity<TeamUser>().ToTable("team_user");
        
        builder.Entity<TeamUser>().HasKey(t => t.Id);
        builder.Entity<TeamUser>()
            .Property(t => t.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => new TeamUserId(value)) 
            .IsRequired();

        builder.Entity<TeamUser>().OwnsOne(t => t.Email, e =>
        {
            e.Property(p => p.Address).HasColumnName("email"); 
        });

        builder.Entity<TeamUser>().Property(t => t.FullName).HasColumnName("full_name");
        builder.Entity<TeamUser>().Property(t => t.Role).HasColumnName("role");
        builder.Entity<TeamUser>().Property(t => t.ProjectId).HasColumnName("project_id");
        builder.Entity<TeamUser>().Property(t => t.IsActive).HasColumnName("is_active");
        
        builder.Entity<TeamUser>().Property(t => t.CreatedAt).HasColumnName("created_at");
        builder.Entity<TeamUser>().Property(t => t.UpdatedAt).HasColumnName("updated_at");
        
        // TEAM WORKERS ---
        
        builder.Entity<TeamWorker>().ToTable("team_worker"); 

        builder.Entity<TeamWorker>().HasKey(w => w.Id);
        builder.Entity<TeamWorker>()
            .Property(w => w.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => new WorkerId(value))
            .IsRequired();

        builder.Entity<TeamWorker>().Property(w => w.Dni).HasColumnName("dni");
        builder.Entity<TeamWorker>().Property(w => w.FullName).HasColumnName("full_name");
        builder.Entity<TeamWorker>().Property(w => w.Role).HasColumnName("role");
        builder.Entity<TeamWorker>().Property(w => w.IsActive).HasColumnName("isActive");
        builder.Entity<TeamWorker>().Property(w => w.ProjectId).HasColumnName("project_id");

        builder.Entity<TeamWorker>().Property(w => w.CreatedAt).HasColumnName("created_at");
        builder.Entity<TeamWorker>().Property(w => w.UpdatedAt).HasColumnName("updated_at");

        builder.Entity<TeamWorker>().OwnsMany(w => w.Machineries, m =>
        {
            m.ToTable("team_workerXmachinery"); 
    
            m.HasKey(wm => wm.Id);
            m.Property(wm => wm.Id).HasColumnName("id").ValueGeneratedOnAdd();
    
            m.WithOwner().HasForeignKey("id_team_worker"); 
    
            m.Property(wm => wm.MachineryId).HasColumnName("id_machinery");
            m.Property(wm => wm.FullName).HasColumnName("fullName");
        });

        builder.Entity<TeamWorker>().Metadata
            .FindNavigation(nameof(TeamWorker.Machineries))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        
        // DOCUMENTS ---
        
        builder.Entity<Document.Domain.Model.Aggregates.Document>().ToTable("document"); 

        builder.Entity<Document.Domain.Model.Aggregates.Document>().HasKey(d => d.Id);
        builder.Entity<Document.Domain.Model.Aggregates.Document>()
            .Property(d => d.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => new DocumentId(value))
            .IsRequired();

        builder.Entity<Document.Domain.Model.Aggregates.Document>().Property(d => d.Type).HasColumnName("type");
        builder.Entity<Document.Domain.Model.Aggregates.Document>().Property(d => d.IsSigned).HasColumnName("is_signed");
        builder.Entity<Document.Domain.Model.Aggregates.Document>().Property(d => d.DigitalSignatureToken).HasColumnName("digital_signature_token");
        builder.Entity<Document.Domain.Model.Aggregates.Document>().Property(d => d.Deadline).HasColumnName("deadline");
        builder.Entity<Document.Domain.Model.Aggregates.Document>().Property(d => d.ProjectId).HasColumnName("project_id");

        builder.Entity<Document.Domain.Model.Aggregates.Document>().Property(d => d.CreatedAt).HasColumnName("created_at");
        builder.Entity<Document.Domain.Model.Aggregates.Document>().Property(d => d.UpdatedAt).HasColumnName("updated_at");

        builder.Entity<Document.Domain.Model.Aggregates.Document>().OwnsMany(d => d.Participants, p =>
        {
            p.ToTable("documentXteam_user"); 
            
            p.HasKey(dp => dp.Id);
            p.Property(dp => dp.Id).HasColumnName("id").ValueGeneratedOnAdd();
            
            p.WithOwner().HasForeignKey("id_document"); 
            
            p.Property(dp => dp.TeamUserId).HasColumnName("id_team_user");
            p.Property(dp => dp.FullName).HasColumnName("full_name");
            p.Property(dp => dp.SignedAt).HasColumnName("signed_at");
        });

        builder.Entity<Document.Domain.Model.Aggregates.Document>().Metadata
            .FindNavigation(nameof(Document.Domain.Model.Aggregates.Document.Participants))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Entity<Kipu.API.Budget.Domain.Model.Aggregates.BudgetItem>(entity =>
        {
            entity.ToTable("budget_items");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Id).ValueGeneratedOnAdd();
            entity.Property(b => b.Details).HasMaxLength(500);
            entity.Property(b => b.AssignedBudget).HasColumnType("decimal(18,2)");
            entity.Property(b => b.ExecutedAmount).HasColumnType("decimal(18,2)");

            entity.OwnsOne(b => b.ActivityName, activity =>
            {
                activity.Property(a => a.Value)
                    .HasColumnName("activity_name")
                    .HasMaxLength(150)
                    .IsRequired();
            });

            entity.HasMany(b => b.Transactions)
                .WithOne()
                .HasForeignKey(t => t.BudgetItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Kipu.API.Budget.Domain.Model.Entities.BudgetTransaction>(entity =>
        {
            entity.ToTable("budget_transactions");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).ValueGeneratedOnAdd();
            entity.Property(t => t.Amount).HasColumnType("decimal(18,2)");
            entity.Property(t => t.Description).HasMaxLength(250);
            entity.Property(t => t.Date).IsRequired();
        });
        
        builder.Entity<Kipu.API.Progress.Domain.Model.Aggregates.ProgressItem>(entity =>
        {
            entity.ToTable("progress_items");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedOnAdd();
            entity.Property(p => p.PlannedPercentage).HasColumnType("decimal(5,2)");
            entity.Property(p => p.ActualPercentage).HasColumnType("decimal(5,2)");
            entity.OwnsOne(p => p.TaskName, tn =>
            {
                tn.Property(t => t.Value).HasColumnName("task_name").HasMaxLength(150).IsRequired();
            });
        });
    }
}
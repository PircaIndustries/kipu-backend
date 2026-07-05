using Kipu.API.Logistics.MaterialWaste.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.MaterialWaste.Domain.Model.Aggregates;

public class MaterialWaste
{
    public MaterialWasteId Id { get; private set; }
    public int ProjectId { get; private set; } // Cambiado a int
    public int MaterialId { get; private set; } // Cambiado a int
    public double Quantity { get; private set; }
    public string ClassificationType { get; private set; }
    public string Date { get; private set; }
    public string Description { get; private set; }
    public string? ReportedBy { get; private set; }
    public string PhotoUrl { get; private set; }

    private MaterialWaste() {}

    public MaterialWaste(
        MaterialWasteId id, 
        int projectId, // Cambiado a int
        int materialId, // Cambiado a int
        double quantity, 
        string classificationType, 
        string description, 
        string? reportedBy, 
        string photoUrl)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (projectId <= 0) throw new ArgumentException("ProjectId must be greater than zero."); // Validación para int
        if (materialId <= 0) throw new ArgumentException("MaterialId must be greater than zero."); // Validación para int
        ArgumentException.ThrowIfNullOrEmpty(classificationType);

        Id = id;
        ProjectId = projectId;
        MaterialId = materialId;
        Quantity = quantity;
        ClassificationType = classificationType.ToUpper();
        Date = DateTime.UtcNow.ToString("yyyy-MM-dd");
        Description = description;
        ReportedBy = reportedBy;
        PhotoUrl = photoUrl ?? string.Empty;
    }

    public void Update(double quantity, string classificationType, string description)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        Quantity = quantity;
        ClassificationType = classificationType.ToUpper();
        Description = description;
    }
}
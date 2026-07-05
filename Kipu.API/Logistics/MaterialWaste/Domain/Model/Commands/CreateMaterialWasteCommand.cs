namespace Kipu.API.Logistics.MaterialWaste.Domain.Model.Commands;

public record CreateMaterialWasteCommand(
    int ProjectId, int MaterialId, double Quantity, 
    string ClassificationType, string Description, string? ReportedBy, string PhotoUrl);

public record UpdateMaterialWasteCommand(
    string Id, double Quantity, string ClassificationType, string Description);

public record DeleteMaterialWasteCommand(string Id);

public record CreateWasteClassificationCommand(string Name);
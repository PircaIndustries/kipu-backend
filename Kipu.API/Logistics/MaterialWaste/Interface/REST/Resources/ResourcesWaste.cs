namespace Kipu.API.Logistics.MaterialWaste.Interface.REST.Resources;

public record CreateMaterialWasteResource(
    int ProjectId, // Cambiado a int
    int MaterialId, // Cambiado a int
    double Quantity, 
    string ClassificationType, 
    string Description, 
    string? ReportedBy, 
    string PhotoUrl);

public record UpdateMaterialWasteResource(
    double Quantity, 
    string ClassificationType, 
    string Description);

public record MaterialWasteResource(
    string Id, 
    int ProjectId, // Cambiado a int
    int MaterialId, // Cambiado a int
    double Quantity, 
    string ClassificationType, 
    string Date, 
    string Description, 
    string? ReportedBy, 
    string PhotoUrl);

public record CreateWasteClassificationResource(string Name);

public record WasteClassificationResource(string Id, string Name);
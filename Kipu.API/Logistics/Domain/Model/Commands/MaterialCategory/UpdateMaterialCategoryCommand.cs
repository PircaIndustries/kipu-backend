using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record UpdateMaterialCategoryCommand(int Id, Name Name, string Description, bool IsActive);

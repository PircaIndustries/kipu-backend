using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record CreateMaterialCategoryCommand(Name Name, String Description, Boolean IsActive);
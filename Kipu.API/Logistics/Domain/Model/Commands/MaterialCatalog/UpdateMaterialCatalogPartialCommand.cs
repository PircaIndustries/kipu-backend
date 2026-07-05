using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record UpdateMaterialCatalogPartialCommand(
    int Id,
    Name? Name = null,
    CategoryId? CategoryId = null,
    MeasureUnit? MeasureUnit = null
);

using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record CreateMaterialCatalogCommand(Name Name, CategoryId CategoryId, MeasureUnit MeasureUnit);
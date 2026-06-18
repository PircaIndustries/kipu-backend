using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Commands;

public record UpdateMaterialCatalogCommand(int Id, Name Name, CategoryId CategoryId, MeasureUnit MeasureUnit);

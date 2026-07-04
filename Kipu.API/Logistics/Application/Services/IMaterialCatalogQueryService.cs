using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;

namespace Kipu.API.Logistics.Application.Services;

public interface IMaterialCatalogQueryService
{
    Task<MaterialCatalog?> Handle(GetMaterialCatalogByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaterialCatalog?>> Handle(GetAllMaterialsCatalogQuery query, CancellationToken cancellationToken = default);
}
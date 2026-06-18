using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Domain.Repositories;

namespace Kipu.API.Logistics.Application.Internal.QueryServices;

public class MaterialCatalogQueryService(IMaterialCatalogRepository materialCatalogRepository)
    : IMaterialCatalogQueryService
{
    public async Task<MaterialCatalog?> Handle(GetMaterialCatalogByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await materialCatalogRepository.FindByIdAsync(query.Id, cancellationToken);
    }

    public async Task<IEnumerable<MaterialCatalog?>> Handle(GetAllMaterialsCatalogQuery query,
        CancellationToken cancellationToken = default)
    {
        return await materialCatalogRepository.ListAsync(cancellationToken);
    }
}
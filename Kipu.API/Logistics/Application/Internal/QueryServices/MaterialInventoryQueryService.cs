using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Domain.Repositories;

namespace Kipu.API.Logistics.Application.Internal.QueryServices;

public class MaterialInventoryQueryService(IMaterialInventoryRepository materialInventoryRepository)
: IMaterialInventoryQueryService
{
    public async Task<IEnumerable<MaterialInventory>> Handle(GetAllMaterialInventoryByCategoryIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await materialInventoryRepository.FindByCategoryIdAsync(query.CategoryId, cancellationToken);
    }

    public async Task<MaterialInventory?> Handle(GetMaterialInventoryByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await materialInventoryRepository.FindByIdAsync(query.Id, cancellationToken);
    }

    public async Task<IEnumerable<MaterialInventory?>> Handle(GetAllMaterialInventoryQuery query,
        CancellationToken cancellationToken = default)
    {
        return await materialInventoryRepository.ListAsync(cancellationToken);
    }
}
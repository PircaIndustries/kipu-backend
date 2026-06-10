using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;

namespace Kipu.API.Logistics.Application.Services;

public interface IMaterialInventoryQueryService
{
    Task<IEnumerable<MaterialInventory>> Handle(GetAllMaterialInventoryByCategoryIdQuery query,
        CancellationToken cancellationToken = default);
    Task<MaterialInventory?> Handle(GetMaterialInventoryByIdQuery query, CancellationToken cancellationToken = default);
}
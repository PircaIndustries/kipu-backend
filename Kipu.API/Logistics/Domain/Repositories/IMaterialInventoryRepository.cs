using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Logistics.Domain.Repositories;

public interface IMaterialInventoryRepository : IBaseRepository<MaterialInventory>
{
    Task<IEnumerable<MaterialInventory>> FindByCategoryIdAsync(CategoryId categoryId, CancellationToken cancellationToken = default);
    Task<MaterialInventory?> FindByMaterialCatalogIdAndProjectIdAsync(int materialCatalogId, int projectId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaterialInventory>> FindByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
}
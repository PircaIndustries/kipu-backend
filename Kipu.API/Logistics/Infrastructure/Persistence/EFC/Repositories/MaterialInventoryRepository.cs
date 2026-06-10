using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Logistics.Infrastructure.Persistence.EFC.Repositories;

public class MaterialInventoryRepository(AppDbContext context)
    : BaseRepository<MaterialInventory>(context), IMaterialInventoryRepository
{
    public async Task<IEnumerable<MaterialInventory>> FindByCategoryIdAsync(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        var query = from inventory in Context.Set<MaterialInventory>()
            join catalog in Context.Set<MaterialCatalog>()
                on inventory.MaterialCatalogId.Value equals catalog.Id 
            where catalog.CategoryId.Value == categoryId.Value
            select inventory;
                    
        return await query.ToListAsync();
    }
}
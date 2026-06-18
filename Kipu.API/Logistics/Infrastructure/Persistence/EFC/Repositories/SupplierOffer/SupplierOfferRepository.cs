using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Logistics.Infrastructure.Persistence.EFC.Repositories;

public class SupplierOfferRepository(AppDbContext context)
    : BaseRepository<SupplierOffer>(context), ISupplierOfferRepository
{
    public async Task<IEnumerable<SupplierOffer>> FindBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default)
    {
        var id = new SupplierId(supplierId);
        return await Context.Set<SupplierOffer>().Where(o => o.SupplierId == id).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SupplierOffer>> FindByMaterialIdAsync(int materialId, CancellationToken cancellationToken = default)
    {
        var id = new MaterialCatalogId(materialId);
        return await Context.Set<SupplierOffer>().Where(o => o.MaterialCatalogId == id).ToListAsync(cancellationToken);
    }
}

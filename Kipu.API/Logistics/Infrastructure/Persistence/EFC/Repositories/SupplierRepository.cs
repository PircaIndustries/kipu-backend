using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Logistics.Infrastructure.Persistence.EFC.Repositories;

public class SupplierRepository(AppDbContext context)
:BaseRepository<Supplier>(context), ISupplierRepository
{
    public async Task<Supplier> FindByRuc(Ruc ruc, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Supplier>().FirstOrDefaultAsync(f => f.Ruc == ruc, cancellationToken);
    }
    public async Task<IEnumerable<Supplier?>> FindByIsActive(Boolean isActive, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Supplier>()
            .Where(f => f.IsActive == isActive)
            .ToListAsync(cancellationToken);
    }
}
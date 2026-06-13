using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Logistics.Infrastructure.Persistence.EFC.Repositories;

public class MaterialRequestRepository(AppDbContext context)
: BaseRepository<MaterialRequest>(context), IMaterialRequestRepository
{
    public async Task<IEnumerable<MaterialRequest>> FindByRequestStatus(RequestStatus requestStatus,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<MaterialRequest>()
            .Include(r => r.Items)
            .Where(f => f.RequestStatus == requestStatus)
            .ToListAsync(cancellationToken);
    }

    public async Task<MaterialRequest?> FindByIdWithItemsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MaterialRequest>()
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<MaterialRequest>> ListWithItemsAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<MaterialRequest>()
            .Include(r => r.Items)
            .ToListAsync(cancellationToken);
    }
}

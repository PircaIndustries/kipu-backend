using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
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
            .Where(f => f.RequestStatus == requestStatus)
            .ToListAsync(cancellationToken);
    }

}
using Kipu.API.Logistics.MaterialWaste.Domain.Model.Aggregates;
using Kipu.API.Logistics.MaterialWaste.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Logistics.MaterialWaste.Infrastructure.Persistence.Repositories;

public class WasteClassificationRepository(AppDbContext context) : IWasteClassificationRepository
{
    public async Task AddAsync(WasteClassification wasteClassification) => 
        await context.Set<WasteClassification>().AddAsync(wasteClassification);

    public async Task<IEnumerable<WasteClassification>> ListAllAsync() => 
        await context.Set<WasteClassification>().ToListAsync();
}
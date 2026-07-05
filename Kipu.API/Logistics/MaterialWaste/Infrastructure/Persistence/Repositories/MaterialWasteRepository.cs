using Kipu.API.Logistics.MaterialWaste.Domain.Model.ValueObjects;
using Kipu.API.Logistics.MaterialWaste.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Logistics.MaterialWaste.Infrastructure.Persistence.Repositories;

public class MaterialWasteRepository(AppDbContext context) 
    : BaseRepository<Domain.Model.Aggregates.MaterialWaste>(context), IMaterialWasteRepository
{
    public async Task AddAsync(Domain.Model.Aggregates.MaterialWaste materialWaste) => await Context.Set<Domain.Model.Aggregates.MaterialWaste>().AddAsync(materialWaste);

    public async Task<Domain.Model.Aggregates.MaterialWaste?> GetByIdAsync(MaterialWasteId id) => 
        await Context.Set<Domain.Model.Aggregates.MaterialWaste>().FirstOrDefaultAsync(w => w.Id == id);

    public async Task<IEnumerable<Domain.Model.Aggregates.MaterialWaste>> ListAllAsync() => await Context.Set<Domain.Model.Aggregates.MaterialWaste>().ToListAsync();

    public void Update(Domain.Model.Aggregates.MaterialWaste materialWaste) => Context.Set<Domain.Model.Aggregates.MaterialWaste>().Update(materialWaste);

    public void Remove(Domain.Model.Aggregates.MaterialWaste materialWaste) => Context.Set<Domain.Model.Aggregates.MaterialWaste>().Remove(materialWaste);
}
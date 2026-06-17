using Kipu.API.NCR.Domain.Model.Aggregates;
using Kipu.API.NCR.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Ncr.Infrastructure.Persistence.EFC.Repositories;

public class NcrRepository : INcrRepository
{
    private readonly AppDbContext _context;

    public NcrRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(NCR.Domain.Model.Aggregates.Ncr entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<NCR.Domain.Model.Aggregates.Ncr>().AddAsync(entity, cancellationToken);
    }

    public async Task<NCR.Domain.Model.Aggregates.Ncr?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<NCR.Domain.Model.Aggregates.Ncr>().FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public void Update(NCR.Domain.Model.Aggregates.Ncr entity)
    {
        _context.Set<NCR.Domain.Model.Aggregates.Ncr>().Update(entity);
    }

    public void Remove(NCR.Domain.Model.Aggregates.Ncr entity)
    {
        _context.Set<NCR.Domain.Model.Aggregates.Ncr>().Remove(entity);
    }

    public async Task<IEnumerable<NCR.Domain.Model.Aggregates.Ncr>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<NCR.Domain.Model.Aggregates.Ncr>().ToListAsync(cancellationToken);
    }
}
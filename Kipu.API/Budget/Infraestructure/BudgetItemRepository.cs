using Kipu.API.Budget.Domain.Model.Aggregates;
using Kipu.API.Budget.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Budget.Infraestructure;

/// <summary>
/// Infrastructure repository implementation for the Budget aggregate context.
/// </summary>
public class BudgetItemRepository(AppDbContext context) 
    : BaseRepository<BudgetItem>(context), IBudgetItemRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<BudgetItem>> FindByProjectIdAsync(int projectId)
    {
        return await Context.Set<BudgetItem>()
            .Where(b => b.ProjectId == projectId)
            .ToListAsync();
    }
}
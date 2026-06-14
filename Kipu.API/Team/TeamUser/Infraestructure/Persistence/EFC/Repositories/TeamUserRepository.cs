using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Kipu.API.Team.TeamUser.domain.model.ValueObjects;
using Kipu.API.Team.TeamUser.domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Team.TeamUser.Infraestructure.Persistence.EFC.Repositories;

public class TeamUserRepository : BaseAggregateRepository<domain.model.Aggregates.TeamUser, UserId>, ITeamUserRepository
{
    public TeamUserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<domain.model.Aggregates.TeamUser>> FindByIsActive(string projectId, bool isActive)
    {
        return await Context.Set<domain.model.Aggregates.TeamUser>()
            .Where(u => u.ProjectId == projectId && u.IsActive == isActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<domain.model.Aggregates.TeamUser>> FindByRole(string projectId, string role)
    {
        return await Context.Set<domain.model.Aggregates.TeamUser>()
            .Where(u => u.ProjectId == projectId && u.Role == role)
            .ToListAsync();
    }

    public async Task<IEnumerable<domain.model.Aggregates.TeamUser>> FindByFilter(string projectId, string? globalSearch, string? role, bool? isActive)
    {
        var query = Context.Set<domain.model.Aggregates.TeamUser>()
            .Where(u => u.ProjectId == projectId)
            .AsQueryable();
            
        
        if (!string.IsNullOrWhiteSpace(globalSearch))
        {
            query = query.Where(u => 
                u.FullName.Contains(globalSearch) || 
                u.Role.Contains(globalSearch) || 
                u.Email.Address.Contains(globalSearch)
            );
        }

        if (!string.IsNullOrWhiteSpace(role))
        {
            query = query.Where(u => u.Role == role);
        }
        
        if (isActive.HasValue)
        {
            query = query.Where(u => u.IsActive == isActive.Value);
        }

        return await query.ToListAsync();
    }
    
}
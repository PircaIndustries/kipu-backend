using Kipu.API.Shared.Domain.Repositories;
using Kipu.API.Team.TeamUser.application.Services;
using Kipu.API.Team.TeamUser.domain.model.Queries;
using Kipu.API.Team.TeamUser.domain.model.ValueObjects;
using Kipu.API.Team.TeamUser.domain.Repositories;

namespace Kipu.API.Team.TeamUser.application.Internal.QueryServices;

public class TeamUserQueryService : ITeamUserQueryService
{
    
    private readonly ITeamUserRepository _teamUserRepository;
    private readonly IUnitOfWork _unitOfWork;


    public TeamUserQueryService(ITeamUserRepository teamUserRepository, IUnitOfWork unitOfWork)
    {
        _teamUserRepository = teamUserRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<domain.model.Aggregates.TeamUser?> Handle(GetTeamUserByIdQuery query)
    {
        return await _teamUserRepository.FindByIdAsync(new UserId(query.Id));
         
    }

    public async Task<IEnumerable<domain.model.Aggregates.TeamUser>> Handle(GetAllTeamUsersByRoleQuery query)
    {
        return await _teamUserRepository.FindByRole(query.ProjectId ,query.Role);
    }

    public async Task<IEnumerable<domain.model.Aggregates.TeamUser>> Handle(GetAllTeamUsersQuery query)
    {
        return await _teamUserRepository.FindByFilter(query.ProjectId, null, null, null);
    }

    public async Task<IEnumerable<domain.model.Aggregates.TeamUser>> Handle(GetAllTeamUsersByIsActiveQuery query)
    {
        return await _teamUserRepository.FindByIsActive(query.projectId,true);
    }

    public Task<IEnumerable<domain.model.Aggregates.TeamUser>> Handle(GetAllTeamUsersByFilterQuery query)
    {
        return _teamUserRepository.FindByFilter(query.ProjectId, query.GlobalSearch, query.Role, query.IsActive);
    }
}
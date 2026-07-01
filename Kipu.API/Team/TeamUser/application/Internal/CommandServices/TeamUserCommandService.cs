using Kipu.API.Shared.Domain.Repositories;
using Kipu.API.Team.TeamUser.application.Services;
using Kipu.API.Team.TeamUser.domain.model.Commands;
using Kipu.API.Team.TeamUser.domain.model.ValueObjects;
using Kipu.API.Team.TeamUser.domain.Repositories;

namespace Kipu.API.Team.TeamUser.application.Internal.CommandServices;

public class TeamUserCommandService : ITeamUserCommandService
{
    private readonly ITeamUserRepository _teamUserRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TeamUserCommandService(ITeamUserRepository teamUserRepository, IUnitOfWork unitOfWork)
    {
        _teamUserRepository = teamUserRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<domain.model.Aggregates.TeamUser?> Handle(CreateTeamUserCommand command)
    {
        var teamUser = new domain.model.Aggregates.TeamUser(
            command.UserId, command.FullName, new Email(command.Email), command.Role, command.ProjectId);

        try
        {
            await _teamUserRepository.AddAsync(teamUser);
            await _unitOfWork.CompleteAsync();
            return teamUser;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while saving the team user: {e.Message}");
            return null;
        }
    }

    public async Task<domain.model.Aggregates.TeamUser?> Handle(ActivateTeamUserCommand command)
    {
        var userId = new UserId(command.Id);
        var teamUser = await _teamUserRepository.FindByIdAsync(userId);

        if (teamUser is null) return null;

        teamUser.Activate();

        try
        {
            _teamUserRepository.Update(teamUser);
            await _unitOfWork.CompleteAsync();
            return teamUser;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error activating team user: {e.Message}");
            return null;
        }
    }

    public async Task<domain.model.Aggregates.TeamUser?> Handle(DeactivateTeamUserCommand command)
    {
        var userId = new UserId(command.Id);
        var teamUser = await _teamUserRepository.FindByIdAsync(userId);

        if (teamUser is null) return null;

        teamUser.Deactivate();

        try
        {
            _teamUserRepository.Update(teamUser);
            await _unitOfWork.CompleteAsync();
            return teamUser;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error deactivating team user: {e.Message}");
            return null;
        }
    }
}

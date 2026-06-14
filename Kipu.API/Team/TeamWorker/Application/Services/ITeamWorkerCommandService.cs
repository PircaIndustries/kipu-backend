using Kipu.API.Team.TeamWorker.Domain.Model.Commands;

namespace Kipu.API.Team.TeamWorker.Application.Services;

public interface ITeamWorkerCommandService
{
    Task<Domain.Model.Aggregates.TeamWorker?> Handle(CreateTeamWorkerCommand command);
    Task<bool> Handle(DeleteTeamWorkerCommand command); 
    Task<Domain.Model.Aggregates.TeamWorker?> Handle(AssignMachineryToTeamWorkerCommand command);
    Task<Domain.Model.Aggregates.TeamWorker?> Handle(RemoveMachineryFromTeamWorkerCommand command);
}
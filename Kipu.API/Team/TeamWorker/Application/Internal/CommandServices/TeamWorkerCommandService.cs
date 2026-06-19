using Kipu.API.Shared.Domain.Repositories;
using Kipu.API.Team.TeamWorker.Application.Services;
using Kipu.API.Team.TeamWorker.Domain.Model.Commands;
using Kipu.API.Team.TeamWorker.Domain.Model.ValueObjects;
using Kipu.API.Team.TeamWorker.Domain.Repositories;

namespace Kipu.API.Team.TeamWorker.Application.Internal.CommandServices;

public class TeamWorkerCommandService : ITeamWorkerCommandService
{
    private readonly ITeamWorkerRepository _teamWorkerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TeamWorkerCommandService(ITeamWorkerRepository teamWorkerRepository, IUnitOfWork unitOfWork)
    {
        _teamWorkerRepository = teamWorkerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Domain.Model.Aggregates.TeamWorker?> Handle(CreateTeamWorkerCommand command)
    {
        var worker = new Domain.Model.Aggregates.TeamWorker(
            command.Dni, 
            command.FullName, 
            command.Role, 
            command.ProjectId
        );

        foreach (var machinery in command.Machineries)
        {
            worker.AssignMachinery(machinery.MachineryId, machinery.FullName);
        }

        try
        {
            await _teamWorkerRepository.AddAsync(worker);
            await _unitOfWork.CompleteAsync();
            return worker;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error happened creating the worker: {e.Message}");
            return null;
        }
    }

    public async Task<bool> Handle(DeleteTeamWorkerCommand command)
    {
        var workerId = new WorkerId(command.TeamWorkerId);
        var worker = await _teamWorkerRepository.FindByIdAsync(workerId); 

        if (worker is null) return false;

        try
        {
            _teamWorkerRepository.Remove(worker);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error happened while deleting the worker: {e.Message}");
            return false;
        }
    }

    public async Task<Domain.Model.Aggregates.TeamWorker?> Handle(AssignMachineryToTeamWorkerCommand command)
    {
        var workerId = new WorkerId(command.TeamWorkerId);
        var worker = await _teamWorkerRepository.FindByIdAsync(workerId);

        if (worker is null) return null;

        try
        {
            worker.AssignMachinery(command.MachineryId, command.FullName);

            _teamWorkerRepository.Update(worker);
            await _unitOfWork.CompleteAsync();
            return worker;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error happened assigning the machinery: {e.Message}");
            return null;
        }
    }

    public async Task<Domain.Model.Aggregates.TeamWorker?> Handle(RemoveMachineryFromTeamWorkerCommand command)
    {
        var workerId = new WorkerId(command.TeamWorkerId);
        var worker = await _teamWorkerRepository.FindByIdAsync(workerId);

        if (worker is null) return null;

        try
        {
            worker.RemoveMachinery(command.MachineryId);

            _teamWorkerRepository.Update(worker);
            await _unitOfWork.CompleteAsync();
            return worker;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error happened deleting the machinery: {e.Message}");
            return null;
        }
    }
}
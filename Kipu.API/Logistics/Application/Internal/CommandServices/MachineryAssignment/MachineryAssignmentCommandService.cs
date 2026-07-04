using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Logistics.Application.Internal.CommandServices;

public class MachineryAssignmentCommandService(
    IMachineryAssignmentRepository machineryAssignmentRepository,
    IUnitOfWork unitOfWork,
    ILogger<MachineryAssignmentCommandService> logger)
    : IMachineryAssignmentCommandService
{
    public async Task<Result<MachineryAssignment, CreateMachineryAssignmentError>> Handle(
        CreateMachineryAssignmentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var assignment = new MachineryAssignment(command);
            await machineryAssignmentRepository.AddAsync(assignment, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MachineryAssignment, CreateMachineryAssignmentError>.Success(assignment);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating machinery assignment");
            return new Result<MachineryAssignment, CreateMachineryAssignmentError>.Failure(
                CreateMachineryAssignmentError.UnexpectedError);
        }
    }

    public async Task<Result<MachineryAssignment, UpdateMachineryAssignmentError>> Handle(
        UpdateMachineryAssignmentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var existing = await machineryAssignmentRepository.FindByIdAsync(command.Id, cancellationToken);
            if (existing is null)
            {
                logger.LogWarning("Machinery assignment {Id} not found for update", command.Id);
                return new Result<MachineryAssignment, UpdateMachineryAssignmentError>.Failure(
                    UpdateMachineryAssignmentError.NotFound);
            }
            existing.Update(command);
            machineryAssignmentRepository.Update(existing);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MachineryAssignment, UpdateMachineryAssignmentError>.Success(existing);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating machinery assignment {Id}", command.Id);
            return new Result<MachineryAssignment, UpdateMachineryAssignmentError>.Failure(
                UpdateMachineryAssignmentError.UnexpectedError);
        }
    }

    public async Task<Result<MachineryAssignment, UpdateMachineryAssignmentError>> HandleDelete(
        string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var existing = await machineryAssignmentRepository.FindByIdAsync(id, cancellationToken);
            if (existing is null)
            {
                logger.LogWarning("Machinery assignment {Id} not found for delete", id);
                return new Result<MachineryAssignment, UpdateMachineryAssignmentError>.Failure(
                    UpdateMachineryAssignmentError.NotFound);
            }
            machineryAssignmentRepository.Remove(existing);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MachineryAssignment, UpdateMachineryAssignmentError>.Success(existing);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting machinery assignment {Id}", id);
            return new Result<MachineryAssignment, UpdateMachineryAssignmentError>.Failure(
                UpdateMachineryAssignmentError.UnexpectedError);
        }
    }
}

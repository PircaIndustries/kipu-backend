using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Logistics.Application.Internal.CommandServices;

public class MaterialRequestCommandService(
    IMaterialRequestRepository materialRequestRepository,
    IUnitOfWork unitOfWork,
    ILogger<MaterialRequestCommandService> logger)
    : IMaterialRequestCommandService
{
    public async Task<Result<MaterialRequest, CreateMaterialRequestError>> Handle(
        CreateMaterialRequestCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new MaterialRequest(command);
            await materialRequestRepository.AddAsync(request, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaterialRequest, CreateMaterialRequestError>.Success(request);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments when creating material request");
            return new Result<MaterialRequest, CreateMaterialRequestError>.Failure(
                CreateMaterialRequestError.UnexpectedError);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating material request");
            return new Result<MaterialRequest, CreateMaterialRequestError>.Failure(
                CreateMaterialRequestError.UnexpectedError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating material request");
            return new Result<MaterialRequest, CreateMaterialRequestError>.Failure(
                CreateMaterialRequestError.UnexpectedError);
        }
    }

    public async Task<Result<MaterialRequest, UpdateMaterialRequestError>> Handle(
        UpdateMaterialRequestCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = await materialRequestRepository.FindByIdWithItemsAsync(command.Id, cancellationToken);
            if (request is null)
            {
                logger.LogWarning("Material request with ID {Id} not found", command.Id);
                return new Result<MaterialRequest, UpdateMaterialRequestError>.Failure(
                    UpdateMaterialRequestError.MaterialRequestNotFound);
            }

            try
            {
                request.Update(command);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("accepted"))
            {
                logger.LogWarning(ex, "Attempted to update accepted request with ID {Id}", command.Id);
                return new Result<MaterialRequest, UpdateMaterialRequestError>.Failure(
                    UpdateMaterialRequestError.RequestAlreadyAccepted);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("refused"))
            {
                logger.LogWarning(ex, "Attempted to update refused request with ID {Id}", command.Id);
                return new Result<MaterialRequest, UpdateMaterialRequestError>.Failure(
                    UpdateMaterialRequestError.RequestAlreadyRefused);
            }

            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaterialRequest, UpdateMaterialRequestError>.Success(request);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments when updating material request with ID {Id}", command.Id);
            return new Result<MaterialRequest, UpdateMaterialRequestError>.Failure(
                UpdateMaterialRequestError.UnexpectedError);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating material request with ID {Id}", command.Id);
            return new Result<MaterialRequest, UpdateMaterialRequestError>.Failure(
                UpdateMaterialRequestError.UnexpectedError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating material request with ID {Id}", command.Id);
            return new Result<MaterialRequest, UpdateMaterialRequestError>.Failure(
                UpdateMaterialRequestError.UnexpectedError);
        }
    }

    public async Task<Result<MaterialRequest, UpdatePartialMaterialRequestError>> Handle(
        UpdateMaterialRequestPartialCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = await materialRequestRepository.FindByIdWithItemsAsync(command.Id, cancellationToken);
            if (request is null)
            {
                logger.LogWarning("Material request with ID {Id} not found", command.Id);
                return new Result<MaterialRequest, UpdatePartialMaterialRequestError>.Failure(
                    UpdatePartialMaterialRequestError.MaterialRequestNotFound);
            }

            try
            {
                request.UpdatePartial(command);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("accepted"))
            {
                logger.LogWarning(ex, "Attempted to patch accepted request with ID {Id}", command.Id);
                return new Result<MaterialRequest, UpdatePartialMaterialRequestError>.Failure(
                    UpdatePartialMaterialRequestError.RequestAlreadyAccepted);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("refused"))
            {
                logger.LogWarning(ex, "Attempted to patch refused request with ID {Id}", command.Id);
                return new Result<MaterialRequest, UpdatePartialMaterialRequestError>.Failure(
                    UpdatePartialMaterialRequestError.RequestAlreadyRefused);
            }

            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaterialRequest, UpdatePartialMaterialRequestError>.Success(request);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments when patching material request with ID {Id}", command.Id);
            return new Result<MaterialRequest, UpdatePartialMaterialRequestError>.Failure(
                UpdatePartialMaterialRequestError.UnexpectedError);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed patching material request with ID {Id}", command.Id);
            return new Result<MaterialRequest, UpdatePartialMaterialRequestError>.Failure(
                UpdatePartialMaterialRequestError.UnexpectedError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error patching material request with ID {Id}", command.Id);
            return new Result<MaterialRequest, UpdatePartialMaterialRequestError>.Failure(
                UpdatePartialMaterialRequestError.UnexpectedError);
        }
    }
}

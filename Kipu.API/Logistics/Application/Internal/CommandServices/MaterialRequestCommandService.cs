using Kipu.API.Budget.Domain.Repositories;
using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Model.ValueObjects.External;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Logistics.Application.Internal.CommandServices;

public class MaterialRequestCommandService(
    IMaterialRequestRepository materialRequestRepository,
    IMaterialInventoryRepository materialInventoryRepository,
    IBudgetItemRepository budgetItemRepository,
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

            if (command.RequestStatus == RequestStatus.Accepted && request.BudgetLineId is not null)
            {
                var budgetItem = await budgetItemRepository.FindByIdAsync(request.BudgetLineId.Value, cancellationToken);
                if (budgetItem is null)
                {
                    return new Result<MaterialRequest, UpdatePartialMaterialRequestError>.Failure(
                        UpdatePartialMaterialRequestError.BudgetExceeded);
                }

                var totalAmount = request.Items.Sum(i => i.Quantity * i.UnitPrice);
                var availableBudget = budgetItem.AssignedBudget - budgetItem.ExecutedAmount;

                if (totalAmount > availableBudget)
                {
                    logger.LogWarning("Budget validation failed for request {Id}: total {Total} exceeds available {Available}",
                        command.Id, totalAmount, availableBudget);
                    return new Result<MaterialRequest, UpdatePartialMaterialRequestError>.Failure(
                        UpdatePartialMaterialRequestError.BudgetExceeded);
                }
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

            if (request.RequestStatus == RequestStatus.Accepted)
            {
                if (request.BudgetLineId is not null)
                {
                    var budgetItem = await budgetItemRepository.FindByIdAsync(request.BudgetLineId.Value, cancellationToken);
                    if (budgetItem is not null)
                    {
                        var totalAmount = request.Items.Sum(i => i.Quantity * i.UnitPrice);
                        logger.LogInformation("Approving request {Id}: registering expense S/ {Amount} on budget line {BudgetId}",
                            request.Id, totalAmount, budgetItem.Id);
                        budgetItem.RegisterExpense(totalAmount, $"MR#{request.Id} approved");
                        budgetItemRepository.Update(budgetItem);
                    }
                    else
                    {
                        logger.LogWarning("Budget line {BudgetId} not found for request {Id} during approval.",
                            request.BudgetLineId.Value, request.Id);
                    }
                }
                else
                {
                    logger.LogInformation("Request {Id} approved without budget line.", request.Id);
                }

                if (request.ProjectId != null)
                {
                    foreach (var item in request.Items)
                    {
                        var existingInventory = await materialInventoryRepository
                            .FindByMaterialCatalogIdAndProjectIdAsync(item.MaterialCatalogId.Value, request.ProjectId.Value, cancellationToken);

                        if (existingInventory != null)
                        {
                            var newStock = new Quantity(existingInventory.CurrentStock.Value + (int)item.Quantity);
                            existingInventory.Update(new UpdateMaterialInventoryCommand(
                                existingInventory.Id,
                                existingInventory.ProjectId,
                                existingInventory.MaterialCatalogId,
                                newStock,
                                existingInventory.MinimumStock,
                                existingInventory.Location));
                        }
                        else
                        {
                            var newInventory = new MaterialInventory(new CreateMaterialInventoryCommand(
                                request.ProjectId,
                                item.MaterialCatalogId,
                                new Quantity((int)item.Quantity),
                                new Quantity(0),
                                new WarehouseLocation(request.DeliveryLocation)));
                            await materialInventoryRepository.AddAsync(newInventory, cancellationToken);
                        }
                    }
                }
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

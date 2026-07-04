using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Kipu.API.Logistics.Application.Internal.CommandServices;

public class MaterialInventoryCommandService(
    IMaterialInventoryRepository materialInventoryRepository,
    IMaterialCatalogRepository materialCatalogRepository,
    IUnitOfWork unitOfWork,
    ILogger<MaterialInventoryCommandService> logger)
: IMaterialInventoryCommandService
{
    public async Task<Result<MaterialInventory, CreateMaterialInventoryError>> Handle(
        CreateMaterialInventoryCommand command, CancellationToken cancellationToken = default)
    {
        
        var catalogItem =
            await materialCatalogRepository.FindByIdAsync(command.MaterialCatalogId.Value, cancellationToken);
        if (catalogItem == null)
        {
            logger.LogWarning("The inventory cannot be created. The material {MaterialId} does not exist in the catalog.", 
                command.MaterialCatalogId.Value);
            return new Result<MaterialInventory, CreateMaterialInventoryError>.Failure(
                CreateMaterialInventoryError.DuplicatedMaterialInventory);
        }
        try
        {
            var materialInventory = new MaterialInventory(command);
            await materialInventoryRepository.AddAsync(materialInventory, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaterialInventory, CreateMaterialInventoryError>.Success(materialInventory);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments when creating inventory for the Material {MaterialId}, ProjectId {ProjectId}, CurrentStock {CurrentStock} and MinimumStock {MinimumStock}", 
                command.MaterialCatalogId.Value,  command.ProjectId, command.CurrentStock, command.MinimumStock);
            return new Result<MaterialInventory, CreateMaterialInventoryError>.Failure(
                CreateMaterialInventoryError.UnexpectedError);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex,
                "Duplicate key violation creating inventory for the Material {MaterialId}, ProjectId {ProjectId}, CurrentStock {CurrentStock} and MinimumStock {MinimumStock}",
                command.MaterialCatalogId.Value,  command.ProjectId, command.CurrentStock, command.MinimumStock);
            return new Result<MaterialInventory, CreateMaterialInventoryError>.Failure(
                CreateMaterialInventoryError.DuplicatedMaterialInventory);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex,
                "Database update failed creating inventory for the Material {MaterialId}, ProjectId {ProjectId}, CurrentStock {CurrentStock} and MinimumStock {MinimumStock}",
                command.MaterialCatalogId.Value,  command.ProjectId, command.CurrentStock, command.MinimumStock);
            return new Result<MaterialInventory, CreateMaterialInventoryError>.Failure(
                CreateMaterialInventoryError.UnexpectedError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error creating inventory for the Material {MaterialId}, ProjectId {ProjectId}, CurrentStock {CurrentStock} and MinimumStock {MinimumStock}",
                command.MaterialCatalogId.Value,  command.ProjectId, command.CurrentStock, command.MinimumStock);
            return new Result<MaterialInventory, CreateMaterialInventoryError>.Failure(
                CreateMaterialInventoryError.UnexpectedError);
        }
    }
    public async Task<Result<MaterialInventory, UpdateMaterialInventoryError>> Handle(
        UpdateMaterialInventoryCommand command, CancellationToken cancellationToken = default)
    {
        var catalogItem =
            await materialCatalogRepository.FindByIdAsync(command.MaterialCatalogId.Value, cancellationToken);
        if (catalogItem == null)
        {
            logger.LogWarning("The inventory cannot be updated. The material {MaterialId} does not exist in the catalog.",
                command.MaterialCatalogId.Value);
            return new Result<MaterialInventory, UpdateMaterialInventoryError>.Failure(
                UpdateMaterialInventoryError.MaterialCatalogNotFound);
        }
        try
        {
            var materialInventory = await materialInventoryRepository.FindByIdAsync(command.Id, cancellationToken);
            if (materialInventory is null)
            {
                logger.LogWarning("Material inventory with ID {Id} not found", command.Id);
                return new Result<MaterialInventory, UpdateMaterialInventoryError>.Failure(
                    UpdateMaterialInventoryError.MaterialInventoryNotFound);
            }
            materialInventory.Update(command);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaterialInventory, UpdateMaterialInventoryError>.Success(materialInventory);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments when updating material inventory with ID {Id}", command.Id);
            return new Result<MaterialInventory, UpdateMaterialInventoryError>.Failure(
                UpdateMaterialInventoryError.UnexpectedError);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex, "Duplicate key violation updating material inventory with ID {Id}", command.Id);
            return new Result<MaterialInventory, UpdateMaterialInventoryError>.Failure(
                UpdateMaterialInventoryError.DuplicatedMaterialInventory);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating material inventory with ID {Id}", command.Id);
            return new Result<MaterialInventory, UpdateMaterialInventoryError>.Failure(
                UpdateMaterialInventoryError.UnexpectedError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating material inventory with ID {Id}", command.Id);
            return new Result<MaterialInventory, UpdateMaterialInventoryError>.Failure(
                UpdateMaterialInventoryError.UnexpectedError);
        }
    }

    public async Task<Result<MaterialInventory, UpdateMaterialInventoryError>> HandleDelete(int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var materialInventory = await materialInventoryRepository.FindByIdAsync(id, cancellationToken);
            if (materialInventory is null)
            {
                logger.LogWarning("Material inventory with ID {Id} not found for deletion", id);
                return new Result<MaterialInventory, UpdateMaterialInventoryError>.Failure(
                    UpdateMaterialInventoryError.MaterialInventoryNotFound);
            }
            materialInventoryRepository.Remove(materialInventory);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaterialInventory, UpdateMaterialInventoryError>.Success(materialInventory);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting material inventory with ID {Id}", id);
            return new Result<MaterialInventory, UpdateMaterialInventoryError>.Failure(
                UpdateMaterialInventoryError.UnexpectedError);
        }
    }

    private static bool IsDuplicateKeyViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (!string.Equals(current.GetType().Name, "MySqlException", StringComparison.Ordinal)) continue;
            var numberProperty = current.GetType().GetProperty("Number");
            if (numberProperty?.PropertyType == typeof(int) &&
                numberProperty.GetValue(current) is int errorCode &&
                errorCode == 1062)
                return true;
        }
        return false;
    }
    
}
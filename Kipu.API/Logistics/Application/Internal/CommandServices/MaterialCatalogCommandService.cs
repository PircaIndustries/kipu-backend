using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Logistics.Application.Internal.CommandServices;

public class MaterialCatalogCommandService(
    IMaterialCatalogRepository materialCatalogRepository,
    IMaterialCategoryRepository materialCategoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<MaterialCatalogCommandService> logger)
: IMaterialCatalogCommandService
{
    public async Task<Result<MaterialCatalog, CreateMaterialCatalogError>> Handle(
        CreateMaterialCatalogCommand command, CancellationToken cancellationToken = default)
    {
        
        var categoryItem =
            await materialCategoryRepository.FindByIdAsync(command.CategoryId.Value, cancellationToken);
        if (categoryItem == null)
        {
            logger.LogWarning("The material catalog cannot be created. The category {CategoryId} does not exist in the categories.", 
                command.CategoryId.Value);
            return new Result<MaterialCatalog, CreateMaterialCatalogError>.Failure(
                CreateMaterialCatalogError.DuplicatedMaterialCatalog);
        }
        try
        {
            var materialCatalog = new MaterialCatalog(command);
            await materialCatalogRepository.AddAsync(materialCatalog, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaterialCatalog, CreateMaterialCatalogError>.Success(materialCatalog);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments when creating inventory for the Name {Name}, CategoryId {CategoryId} and MeasureUnit {MeasureUnit}", 
                command.Name,  command.CategoryId, command.MeasureUnit);
            return new Result<MaterialCatalog, CreateMaterialCatalogError>.Failure(
                CreateMaterialCatalogError.UnexpectedError);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex,
                "Duplicate key violation creating inventory for the Name {Name}, CategoryId {CategoryId} and MeasureUnit {MeasureUnit}",
                command.Name,  command.CategoryId, command.MeasureUnit);
            return new Result<MaterialCatalog, CreateMaterialCatalogError>.Failure(
                CreateMaterialCatalogError.DuplicatedMaterialCatalog);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex,
                "Database update failed creating inventory for the Name {Name}, CategoryId {CategoryId} and MeasureUnit {MeasureUnit}",
                command.Name,  command.CategoryId, command.MeasureUnit);
            return new Result<MaterialCatalog, CreateMaterialCatalogError>.Failure(
                CreateMaterialCatalogError.UnexpectedError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error creating inventory for the Name {Name}, CategoryId {CategoryId} and MeasureUnit {MeasureUnit}",
                command.Name,  command.CategoryId, command.MeasureUnit);
            return new Result<MaterialCatalog, CreateMaterialCatalogError>.Failure(
                CreateMaterialCatalogError.UnexpectedError);
        }
    }
    public async Task<Result<MaterialCatalog, UpdateMaterialCatalogError>> Handle(
        UpdateMaterialCatalogCommand command, CancellationToken cancellationToken = default)
    {
        var categoryItem = await materialCategoryRepository.FindByIdAsync(command.CategoryId.Value, cancellationToken);
        if (categoryItem == null)
        {
            logger.LogWarning("The material catalog cannot be updated. The category {CategoryId} does not exist.",
                command.CategoryId.Value);
            return new Result<MaterialCatalog, UpdateMaterialCatalogError>.Failure(
                UpdateMaterialCatalogError.CategoryNotFound);
        }
        try
        {
            var materialCatalog = await materialCatalogRepository.FindByIdAsync(command.Id, cancellationToken);
            if (materialCatalog is null)
            {
                logger.LogWarning("Material catalog with ID {Id} not found", command.Id);
                return new Result<MaterialCatalog, UpdateMaterialCatalogError>.Failure(
                    UpdateMaterialCatalogError.MaterialCatalogNotFound);
            }
            materialCatalog.Update(command);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaterialCatalog, UpdateMaterialCatalogError>.Success(materialCatalog);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments when updating material catalog with ID {Id}", command.Id);
            return new Result<MaterialCatalog, UpdateMaterialCatalogError>.Failure(
                UpdateMaterialCatalogError.UnexpectedError);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex, "Duplicate key violation updating material catalog with ID {Id}", command.Id);
            return new Result<MaterialCatalog, UpdateMaterialCatalogError>.Failure(
                UpdateMaterialCatalogError.DuplicatedMaterialCatalog);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating material catalog with ID {Id}", command.Id);
            return new Result<MaterialCatalog, UpdateMaterialCatalogError>.Failure(
                UpdateMaterialCatalogError.UnexpectedError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating material catalog with ID {Id}", command.Id);
            return new Result<MaterialCatalog, UpdateMaterialCatalogError>.Failure(
                UpdateMaterialCatalogError.UnexpectedError);
        }
    }

    public async Task<Result<MaterialCatalog, UpdateMaterialCatalogError>> HandleDelete(int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var materialCatalog = await materialCatalogRepository.FindByIdAsync(id, cancellationToken);
            if (materialCatalog is null)
            {
                logger.LogWarning("Material catalog with ID {Id} not found for deletion", id);
                return new Result<MaterialCatalog, UpdateMaterialCatalogError>.Failure(
                    UpdateMaterialCatalogError.MaterialCatalogNotFound);
            }
            materialCatalogRepository.Remove(materialCatalog);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaterialCatalog, UpdateMaterialCatalogError>.Success(materialCatalog);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting material catalog with ID {Id}", id);
            return new Result<MaterialCatalog, UpdateMaterialCatalogError>.Failure(
                UpdateMaterialCatalogError.UnexpectedError);
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
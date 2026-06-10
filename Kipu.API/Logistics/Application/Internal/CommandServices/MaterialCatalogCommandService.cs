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
    ILogger<MaterialInventoryCommandService> logger)
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
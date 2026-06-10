using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Kipu.API.Logistics.Application.Internal.CommandServices;

public class MaterialCategoryCommandService(
    IMaterialCategoryRepository materialCategoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<MaterialCategoryCommandService> logger)
: IMaterialCategoryCommandService
{
    public async Task<Result<MaterialCategory, CreateMaterialCategoryError>> Handle(
        CreateMaterialCategoryCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var materialCategory = new MaterialCategory(command);
            await materialCategoryRepository.AddAsync(materialCategory, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaterialCategory, CreateMaterialCategoryError>.Success(materialCategory);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments when creating category for Name {Name}, Description {Description}, IsActive {IsActive}", 
                command.Name, command.Description, command.IsActive);
            return new Result<MaterialCategory, CreateMaterialCategoryError>.Failure(
                CreateMaterialCategoryError.UnexpectedError);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex,
                "Duplicate key violation creating inventory for the Name {Name}",
                command.Name);
            return new Result<MaterialCategory, CreateMaterialCategoryError>.Failure(
                CreateMaterialCategoryError.DuplicatedMaterialCategory);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex,
                "Database update failed creating category for Name {Name}",
                command.Name);
            return new Result<MaterialCategory, CreateMaterialCategoryError>.Failure(
                CreateMaterialCategoryError.UnexpectedError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error creating category for Name {Name}",
                command.Name);
            return new Result<MaterialCategory, CreateMaterialCategoryError>.Failure(
                CreateMaterialCategoryError.UnexpectedError);
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
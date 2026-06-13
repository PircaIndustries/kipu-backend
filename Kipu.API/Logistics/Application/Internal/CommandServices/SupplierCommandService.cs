using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Kipu.API.Logistics.Application.Internal.CommandServices;

public class SupplierCommandService(
    ISupplierRepository supplierRepository,
    IUnitOfWork unitOfWork,
    ILogger<SupplierCommandService> logger)
: ISupplierCommandService
{
    public async Task<Result<Supplier, CreateSupplierError>> Handle(
        CreateSupplierCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingSupplier = await supplierRepository.FindByRuc(command.Ruc, cancellationToken);
            if (existingSupplier is not null)
            {
                logger.LogWarning("Supplier with RUC {Ruc} already exists", command.Ruc);
                return new Result<Supplier, CreateSupplierError>.Failure(
                    CreateSupplierError.DuplicatedSupplier);
            }
            var supplier = new Supplier(command);
            await supplierRepository.AddAsync(supplier, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Supplier, CreateSupplierError>.Success(supplier);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments when creating supplier for RUC {Ruc}, SocialReason {SocialReason}", 
                command.Ruc, command.SocialReason);
            return new Result<Supplier, CreateSupplierError>.Failure(
                CreateSupplierError.UnexpectedError);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex,
                "Duplicate key violation creating supplier for RUC {Ruc}",
                command.Ruc);
            return new Result<Supplier, CreateSupplierError>.Failure(
                CreateSupplierError.DuplicatedSupplier);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex,
                "Database update failed creating supplier for RUC {Ruc}",
                command.Ruc);
            return new Result<Supplier, CreateSupplierError>.Failure(
                CreateSupplierError.UnexpectedError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error creating supplier for RUC {Ruc}",
                command.Ruc);
            return new Result<Supplier, CreateSupplierError>.Failure(
                CreateSupplierError.UnexpectedError);
        }
    }
    
    public async Task<Result<Supplier, UpdateSupplierError>> Handle(
        UpdateSupplierCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            
            var supplier = await supplierRepository.FindByIdAsync(command.Id, cancellationToken);
            
            if (supplier is null)
            {
                logger.LogWarning("Supplier with ID {Id} not found", command.Id);
                return new Result<Supplier, UpdateSupplierError>.Failure(
                    UpdateSupplierError.SupplierNotFound);
            }
            var existingSupplier = await supplierRepository.FindByRuc(command.Ruc, cancellationToken);
            if (existingSupplier is not null && existingSupplier.Id != command.Id)
            {
                logger.LogWarning("Supplier with RUC {Ruc} already exists for different supplier (ID {ExistingId})",
                    command.Ruc, existingSupplier.Id);
                return new Result<Supplier, UpdateSupplierError>.Failure(
                    UpdateSupplierError.DuplicatedRuc);
            }
            supplier.Update(command);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Supplier, UpdateSupplierError>.Success(supplier);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments when updating supplier with ID {Id}", command.Id);
            return new Result<Supplier, UpdateSupplierError>.Failure(
                UpdateSupplierError.UnexpectedError);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex, "Duplicate key violation updating supplier with ID {Id}", command.Id);
            return new Result<Supplier, UpdateSupplierError>.Failure(
                UpdateSupplierError.DuplicatedRuc);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating supplier with ID {Id}", command.Id);
            return new Result<Supplier, UpdateSupplierError>.Failure(
                UpdateSupplierError.UnexpectedError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating supplier with ID {Id}", command.Id);
            return new Result<Supplier, UpdateSupplierError>.Failure(
                UpdateSupplierError.UnexpectedError);
        }
    }

    public async Task<Result<Supplier, UpdatePartialSupplierError>> Handle(UpdateSupplierPartialCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            
            var supplier = await supplierRepository.FindByIdAsync(command.Id, cancellationToken);
            
            if (supplier is null)
            {
                logger.LogWarning("Supplier with ID {Id} not found", command.Id);
                return new Result<Supplier, UpdatePartialSupplierError>.Failure(
                    UpdatePartialSupplierError.SupplierNotFound);
            }
            var existingSupplier = await supplierRepository.FindByRuc(command.Ruc, cancellationToken);
            if (existingSupplier is not null && existingSupplier.Id != command.Id)
            {
                logger.LogWarning("Supplier with RUC {Ruc} already exists for different supplier (ID {ExistingId})",
                    command.Ruc, existingSupplier.Id);
                return new Result<Supplier, UpdatePartialSupplierError>.Failure(
                    UpdatePartialSupplierError.DuplicatedRuc);
            }
            supplier.UpdatePartial(command);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Supplier, UpdatePartialSupplierError>.Success(supplier);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments when updating supplier with ID {Id}", command.Id);
            return new Result<Supplier, UpdatePartialSupplierError>.Failure(
                UpdatePartialSupplierError.UnexpectedError);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex, "Duplicate key violation updating supplier with ID {Id}", command.Id);
            return new Result<Supplier, UpdatePartialSupplierError>.Failure(
                UpdatePartialSupplierError.DuplicatedRuc);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating supplier with ID {Id}", command.Id);
            return new Result<Supplier, UpdatePartialSupplierError>.Failure(
                UpdatePartialSupplierError.UnexpectedError);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating supplier with ID {Id}", command.Id);
            return new Result<Supplier, UpdatePartialSupplierError>.Failure(
                UpdatePartialSupplierError.UnexpectedError);
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
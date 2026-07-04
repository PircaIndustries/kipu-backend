using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Logistics.Application.Internal.CommandServices;

public class SupplierOfferCommandService(
    ISupplierOfferRepository supplierOfferRepository,
    IUnitOfWork unitOfWork,
    ILogger<SupplierOfferCommandService> logger)
    : ISupplierOfferCommandService
{
    public async Task<Result<SupplierOffer, CreateSupplierOfferError>> Handle(
        CreateSupplierOfferCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var supplierOffer = new SupplierOffer(command);
            await supplierOfferRepository.AddAsync(supplierOffer, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<SupplierOffer, CreateSupplierOfferError>.Success(supplierOffer);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            return new Result<SupplierOffer, CreateSupplierOfferError>.Failure(
                CreateSupplierOfferError.DuplicatedSupplierOffer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating supplier offer");
            return new Result<SupplierOffer, CreateSupplierOfferError>.Failure(
                CreateSupplierOfferError.UnexpectedError);
        }
    }

    public async Task<Result<SupplierOffer, UpdateSupplierOfferError>> Handle(
        UpdateSupplierOfferCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var offer = await supplierOfferRepository.FindByIdAsync(command.Id, cancellationToken);
            if (offer is null)
                return new Result<SupplierOffer, UpdateSupplierOfferError>.Failure(
                    UpdateSupplierOfferError.SupplierOfferNotFound);

            offer.Update(command);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<SupplierOffer, UpdateSupplierOfferError>.Success(offer);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            return new Result<SupplierOffer, UpdateSupplierOfferError>.Failure(
                UpdateSupplierOfferError.DuplicatedSupplierOffer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating supplier offer");
            return new Result<SupplierOffer, UpdateSupplierOfferError>.Failure(
                UpdateSupplierOfferError.UnexpectedError);
        }
    }

    public async Task<Result<SupplierOffer, UpdateSupplierOfferError>> HandleDelete(int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var offer = await supplierOfferRepository.FindByIdAsync(id, cancellationToken);
            if (offer is null)
                return new Result<SupplierOffer, UpdateSupplierOfferError>.Failure(
                    UpdateSupplierOfferError.SupplierOfferNotFound);
            supplierOfferRepository.Remove(offer);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<SupplierOffer, UpdateSupplierOfferError>.Success(offer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting supplier offer");
            return new Result<SupplierOffer, UpdateSupplierOfferError>.Failure(
                UpdateSupplierOfferError.UnexpectedError);
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

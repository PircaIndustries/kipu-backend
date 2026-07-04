using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Shared.Application.Patterns;

namespace Kipu.API.Logistics.Application.Services;

public interface ISupplierOfferCommandService
{
    Task<Result<SupplierOffer, CreateSupplierOfferError>> Handle(CreateSupplierOfferCommand command, CancellationToken cancellationToken = default);
    Task<Result<SupplierOffer, UpdateSupplierOfferError>> Handle(UpdateSupplierOfferCommand command, CancellationToken cancellationToken = default);
    Task<Result<SupplierOffer, UpdateSupplierOfferError>> HandleDelete(int id, CancellationToken cancellationToken = default);
}

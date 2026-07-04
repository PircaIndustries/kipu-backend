using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Domain.Repositories;

namespace Kipu.API.Logistics.Application.Internal.QueryServices;

public class SupplierOfferQueryService(ISupplierOfferRepository supplierOfferRepository)
    : ISupplierOfferQueryService
{
    public async Task<SupplierOffer?> Handle(GetSupplierOfferByIdQuery query, CancellationToken cancellationToken = default)
        => await supplierOfferRepository.FindByIdAsync(query.Id, cancellationToken);

    public async Task<IEnumerable<SupplierOffer>> Handle(GetAllSupplierOffersQuery query, CancellationToken cancellationToken = default)
        => await supplierOfferRepository.ListAsync(cancellationToken);

    public async Task<IEnumerable<SupplierOffer>> Handle(GetSupplierOffersBySupplierIdQuery query, CancellationToken cancellationToken = default)
        => await supplierOfferRepository.FindBySupplierIdAsync(query.SupplierId.Value, cancellationToken);

    public async Task<IEnumerable<SupplierOffer>> Handle(GetSupplierOffersByMaterialIdQuery query, CancellationToken cancellationToken = default)
        => await supplierOfferRepository.FindByMaterialIdAsync(query.MaterialCatalogId.Value, cancellationToken);
}

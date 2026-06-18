using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;

namespace Kipu.API.Logistics.Application.Services;

public interface ISupplierOfferQueryService
{
    Task<SupplierOffer?> Handle(GetSupplierOfferByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<SupplierOffer>> Handle(GetAllSupplierOffersQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<SupplierOffer>> Handle(GetSupplierOffersBySupplierIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<SupplierOffer>> Handle(GetSupplierOffersByMaterialIdQuery query, CancellationToken cancellationToken = default);
}

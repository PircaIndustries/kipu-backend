using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Logistics.Domain.Repositories;

public interface ISupplierOfferRepository : IBaseRepository<SupplierOffer>
{
    Task<IEnumerable<SupplierOffer>> FindBySupplierIdAsync(int supplierId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SupplierOffer>> FindByMaterialIdAsync(int materialId, CancellationToken cancellationToken = default);
}

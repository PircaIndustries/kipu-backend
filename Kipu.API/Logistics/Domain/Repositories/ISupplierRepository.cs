using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Logistics.Domain.Repositories;

public interface ISupplierRepository : IBaseRepository<Supplier>
{
    Task<IEnumerable<Supplier>> FindByRuc(Ruc ruc, CancellationToken cancellationToken = default);
    Task<IEnumerable<Supplier>> FindByIsActive(Boolean isActive, CancellationToken cancellationToken = default);

}
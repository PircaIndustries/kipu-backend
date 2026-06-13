using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Domain.Repositories;

namespace Kipu.API.Logistics.Application.Internal.QueryServices;

public class SupplierQueryService(ISupplierRepository supplierRepository)
    : ISupplierQueryService
{
    public async Task<Supplier?> Handle(GetSupplierByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await supplierRepository.FindByIdAsync(query.id, cancellationToken);
    }

    public async Task<Supplier?> Handle(GetAllSupplierByRucQuery query, CancellationToken cancellationToken = default)
    {
        return await supplierRepository.FindByRuc(query.Ruc, cancellationToken);
    }

    public async Task<IEnumerable<Supplier?>> Handle(GetAllSupplierByIsActiveQuery query,
        CancellationToken cancellationToken = default)
    {
        return await supplierRepository.FindByIsActive(query.IsActive, cancellationToken);
    }
}

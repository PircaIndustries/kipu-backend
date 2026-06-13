using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;

namespace Kipu.API.Logistics.Application.Services;

public interface ISupplierQueryService
{
    Task<Supplier?> Handle(GetSupplierByIdQuery query, CancellationToken cancellationToken = default);
    Task<Supplier?> Handle(GetAllSupplierByRucQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Supplier?>> Handle(GetAllSupplierByIsActiveQuery query, CancellationToken cancellationToken = default);
}
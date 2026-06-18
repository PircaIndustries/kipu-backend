using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Shared.Application.Patterns;

namespace Kipu.API.Logistics.Application.Services;

public interface ISupplierCommandService
{
    Task<Result<Supplier, CreateSupplierError>> Handle(CreateSupplierCommand command, CancellationToken cancellationToken = default);
    Task<Result<Supplier, UpdateSupplierError>> Handle(UpdateSupplierCommand command, CancellationToken cancellationToken = default);
    Task<Result<Supplier, UpdatePartialSupplierError>> Handle(UpdateSupplierPartialCommand command, CancellationToken cancellationToken = default);
    Task<Result<Supplier, UpdateSupplierError>> HandleDelete(int id, CancellationToken cancellationToken = default);

}
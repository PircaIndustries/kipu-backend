using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Shared.Application.Patterns;

namespace Kipu.API.Logistics.Application.Services;

public interface IMaterialInventoryCommandService
{
    Task<Result<MaterialInventory, CreateMaterialInventoryError>> Handle(CreateMaterialInventoryCommand command, CancellationToken cancellationToken = default);
    Task<Result<MaterialInventory, UpdateMaterialInventoryError>> Handle(UpdateMaterialInventoryCommand command, CancellationToken cancellationToken = default);
    Task<Result<MaterialInventory, UpdateMaterialInventoryError>> HandleDelete(int id, CancellationToken cancellationToken = default);
}
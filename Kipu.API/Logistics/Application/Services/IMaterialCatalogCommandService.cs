using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Shared.Application.Patterns;

namespace Kipu.API.Logistics.Application.Services;

public interface IMaterialCatalogCommandService
{
    Task<Result<MaterialCatalog, CreateMaterialCatalogError>> Handle(CreateMaterialCatalogCommand command, CancellationToken cancellationToken = default);
    Task<Result<MaterialCatalog, UpdateMaterialCatalogError>> Handle(UpdateMaterialCatalogCommand command, CancellationToken cancellationToken = default);
    Task<Result<MaterialCatalog, UpdateMaterialCatalogError>> HandleDelete(int id, CancellationToken cancellationToken = default);
}
using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Shared.Application.Patterns;

namespace Kipu.API.Logistics.Application.Services;

public interface IMaterialCategoryCommandService
{
    Task<Result<MaterialCategory, CreateMaterialCategoryError>> Handle(CreateMaterialCategoryCommand command, CancellationToken cancellationToken = default);
}
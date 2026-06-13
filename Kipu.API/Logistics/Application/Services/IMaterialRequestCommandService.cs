using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Shared.Application.Patterns;

namespace Kipu.API.Logistics.Application.Services;

public interface IMaterialRequestCommandService
{
    Task<Result<MaterialRequest, CreateMaterialRequestError>> Handle(CreateMaterialRequestCommand command, CancellationToken cancellationToken = default);
    Task<Result<MaterialRequest, UpdateMaterialRequestError>> Handle(UpdateMaterialRequestCommand command, CancellationToken cancellationToken = default);
    Task<Result<MaterialRequest, UpdatePartialMaterialRequestError>> Handle(UpdateMaterialRequestPartialCommand command, CancellationToken cancellationToken = default);
}

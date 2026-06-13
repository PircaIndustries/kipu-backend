using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Interfaces.REST.Resources.Supplier;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Kipu.API.Resources;
using SupplierEntity = Kipu.API.Logistics.Domain.Model.Aggregates.Supplier;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.Supplier;

public static class ActionResultFromUpdateSupplierAssembler
{
    public static ActionResult ToActionResultFromUpdateSupplierResult(
        Result<SupplierEntity, UpdateSupplierError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer)
    {
        return result switch
        {
            Result<SupplierEntity, UpdateSupplierError>.Success success =>
                controller.Ok(SupplierResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<SupplierEntity, UpdateSupplierError>.Failure failure =>
                failure.Error switch
                {
                    UpdateSupplierError.SupplierNotFound =>
                        controller.NotFound(localizer["SupplierNotFound"].Value),

                    UpdateSupplierError.DuplicatedRuc =>
                        controller.Conflict(localizer["SupplierDuplicatedRuc"].Value),

                    UpdateSupplierError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingSupplier"].Value,
                            statusCode: 500),

                    _ => controller.Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                        statusCode: 500)
                },

            _ => controller.Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: 500)
        };
    }
}
using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Interfaces.REST.Resources.Supplier;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Kipu.API.Resources;
using SupplierEntity = Kipu.API.Logistics.Domain.Model.Aggregates.Supplier;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.Supplier;

public static class ActionResultFromUpdatePartialSupplierAssembler
{
    public static ActionResult ToActionResultFromUpdatePartialSupplierResult(
        Result<SupplierEntity, UpdatePartialSupplierError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer)
    {
        return result switch
        {
            Result<SupplierEntity, UpdatePartialSupplierError>.Success success =>
                controller.Ok(SupplierResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<SupplierEntity, UpdatePartialSupplierError>.Failure failure =>
                failure.Error switch
                {
                    UpdatePartialSupplierError.SupplierNotFound =>
                        controller.NotFound(localizer["SupplierNotFound"].Value),

                    UpdatePartialSupplierError.DuplicatedRuc =>
                        controller.Conflict(localizer["SupplierDuplicatedRuc"].Value),

                    UpdatePartialSupplierError.UnexpectedError =>
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
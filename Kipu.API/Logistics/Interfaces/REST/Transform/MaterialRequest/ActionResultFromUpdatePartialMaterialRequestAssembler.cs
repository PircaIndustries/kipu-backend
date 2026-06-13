using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RequestEntity = Kipu.API.Logistics.Domain.Model.Aggregates.MaterialRequest;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialRequest;

public static class ActionResultFromUpdatePartialMaterialRequestAssembler
{
    public static ActionResult ToActionResultFromUpdatePartialMaterialRequestResult(
        Result<RequestEntity, UpdatePartialMaterialRequestError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<RequestEntity, UpdatePartialMaterialRequestError>.Success success =>
                controller.Ok(MaterialRequestResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<RequestEntity, UpdatePartialMaterialRequestError>.Failure failure =>
                failure.Error switch
                {
                    UpdatePartialMaterialRequestError.MaterialRequestNotFound =>
                        controller.NotFound(localizer["MaterialRequestNotFound"].Value),

                    UpdatePartialMaterialRequestError.RequestAlreadyAccepted =>
                        controller.Conflict(localizer["MaterialRequestAlreadyAccepted"].Value),

                    UpdatePartialMaterialRequestError.RequestAlreadyRefused =>
                        controller.Conflict(localizer["MaterialRequestAlreadyRefused"].Value),

                    UpdatePartialMaterialRequestError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                            statusCode: 500
                        ),

                    _ => controller.Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                        statusCode: 500
                    )
                },

            _ => controller.Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: 500
            )
        };
}

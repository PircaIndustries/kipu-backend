using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RequestEntity = Kipu.API.Logistics.Domain.Model.Aggregates.MaterialRequest;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialRequest;

public static class ActionResultFromUpdateMaterialRequestAssembler
{
    public static ActionResult ToActionResultFromUpdateMaterialRequestResult(
        Result<RequestEntity, UpdateMaterialRequestError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<RequestEntity, UpdateMaterialRequestError>.Success success =>
                controller.Ok(MaterialRequestResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<RequestEntity, UpdateMaterialRequestError>.Failure failure =>
                failure.Error switch
                {
                    UpdateMaterialRequestError.MaterialRequestNotFound =>
                        controller.NotFound(localizer["MaterialRequestNotFound"].Value),

                    UpdateMaterialRequestError.RequestAlreadyAccepted =>
                        controller.Conflict(localizer["MaterialRequestAlreadyAccepted"].Value),

                    UpdateMaterialRequestError.RequestAlreadyRefused =>
                        controller.Conflict(localizer["MaterialRequestAlreadyRefused"].Value),

                    UpdateMaterialRequestError.UnexpectedError =>
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

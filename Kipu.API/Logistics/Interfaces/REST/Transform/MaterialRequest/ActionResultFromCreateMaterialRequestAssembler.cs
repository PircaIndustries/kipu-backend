using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Interfaces.REST.Resources.MaterialRequest;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RequestEntity = Kipu.API.Logistics.Domain.Model.Aggregates.MaterialRequest;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialRequest;

public static class ActionResultFromCreateMaterialRequestAssembler
{
    public static ActionResult ToActionResultFromCreateMaterialRequestResult(
        Result<RequestEntity, CreateMaterialRequestError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getMaterialRequestByIdActionName) =>
        result switch
        {
            Result<RequestEntity, CreateMaterialRequestError>.Success success =>
                controller.CreatedAtAction(
                    getMaterialRequestByIdActionName,
                    new { id = success.Value.Id },
                    MaterialRequestResourceFromEntityAssembler.ToResourceFromEntity(success.Value)
                ),

            Result<RequestEntity, CreateMaterialRequestError>.Failure failure =>
                failure.Error switch
                {
                    CreateMaterialRequestError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingMaterialRequest"].Value,
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

using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Kipu.API.Logistics.Interfaces.REST.Transform;

public static class ActionResultFromCreateMaterialInventoryAssembler
{
    public static ActionResult ToActionResultFromCreateMaterialInventoryResult(
        Result<MaterialInventory, CreateMaterialInventoryError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getMaterialInventoryByCategoryIdActionName) =>
        result switch
        {
            Result<MaterialInventory, CreateMaterialInventoryError>.Success success =>
                controller.CreatedAtAction(getMaterialInventoryByCategoryIdActionName, new { id = success.Value.Id },
                    MaterialInventoryResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<MaterialInventory, CreateMaterialInventoryError>.Failure failure =>
                failure.Error switch
                {
                    CreateMaterialInventoryError.DuplicatedMaterialInventory =>
                        controller.Conflict(localizer["MaterialInventoryDuplicated"].Value),

                    CreateMaterialInventoryError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingMaterialInventory"].Value,
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
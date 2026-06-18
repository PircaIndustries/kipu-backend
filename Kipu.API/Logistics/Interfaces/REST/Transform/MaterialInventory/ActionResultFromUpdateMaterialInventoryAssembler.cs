using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MaterialInventoryEntity = Kipu.API.Logistics.Domain.Model.Aggregates.MaterialInventory;

namespace Kipu.API.Logistics.Interfaces.REST.Transform;

public static class ActionResultFromUpdateMaterialInventoryAssembler
{
    public static ActionResult ToActionResultFromUpdateMaterialInventoryResult(
        Result<MaterialInventoryEntity, UpdateMaterialInventoryError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<MaterialInventoryEntity, UpdateMaterialInventoryError>.Success success =>
                controller.Ok(MaterialInventoryResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<MaterialInventoryEntity, UpdateMaterialInventoryError>.Failure failure =>
                failure.Error switch
                {
                    UpdateMaterialInventoryError.MaterialInventoryNotFound =>
                        controller.NotFound(localizer["MaterialInventoryNotFound"].Value),

                    UpdateMaterialInventoryError.MaterialCatalogNotFound =>
                        controller.NotFound(localizer["MaterialCatalogNotFound"].Value),

                    UpdateMaterialInventoryError.DuplicatedMaterialInventory =>
                        controller.Conflict(localizer["MaterialInventoryDuplicated"].Value),

                    UpdateMaterialInventoryError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingMaterialInventory"].Value,
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

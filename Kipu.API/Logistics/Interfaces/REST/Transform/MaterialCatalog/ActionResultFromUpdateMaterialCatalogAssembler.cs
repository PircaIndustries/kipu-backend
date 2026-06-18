using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MaterialCatalogEntity = Kipu.API.Logistics.Domain.Model.Aggregates.MaterialCatalog;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCatalog;

public static class ActionResultFromUpdateMaterialCatalogAssembler
{
    public static ActionResult ToActionResultFromUpdateMaterialCatalogResult(
        Result<MaterialCatalogEntity, UpdateMaterialCatalogError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<MaterialCatalogEntity, UpdateMaterialCatalogError>.Success success =>
                controller.Ok(MaterialCatalogResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<MaterialCatalogEntity, UpdateMaterialCatalogError>.Failure failure =>
                failure.Error switch
                {
                    UpdateMaterialCatalogError.MaterialCatalogNotFound =>
                        controller.NotFound(localizer["MaterialCatalogNotFound"].Value),

                    UpdateMaterialCatalogError.CategoryNotFound =>
                        controller.NotFound(localizer["CategoryNotFound"].Value),

                    UpdateMaterialCatalogError.DuplicatedMaterialCatalog =>
                        controller.Conflict(localizer["MaterialCatalogDuplicated"].Value),

                    UpdateMaterialCatalogError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingMaterialCatalog"].Value,
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

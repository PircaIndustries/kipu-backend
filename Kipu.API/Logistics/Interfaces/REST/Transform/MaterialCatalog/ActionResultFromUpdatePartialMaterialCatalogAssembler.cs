using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Logistics.Interfaces.REST.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Kipu.API.Resources;
using MaterialCatalogEntity = Kipu.API.Logistics.Domain.Model.Aggregates.MaterialCatalog;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCatalog;

public static class ActionResultFromUpdatePartialMaterialCatalogAssembler
{
    public static ActionResult ToActionResultFromUpdatePartialMaterialCatalogResult(
        Result<MaterialCatalogEntity, UpdatePartialMaterialCatalogError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer)
    {
        return result switch
        {
            Result<MaterialCatalogEntity, UpdatePartialMaterialCatalogError>.Success success =>
                controller.Ok(MaterialCatalogResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<MaterialCatalogEntity, UpdatePartialMaterialCatalogError>.Failure failure =>
                failure.Error switch
                {
                    UpdatePartialMaterialCatalogError.MaterialCatalogNotFound =>
                        controller.NotFound(localizer["MaterialCatalogNotFound"].Value),

                    UpdatePartialMaterialCatalogError.CategoryNotFound =>
                        controller.BadRequest(localizer["CategoryNotFound"].Value),

                    UpdatePartialMaterialCatalogError.DuplicatedMaterialCatalog =>
                        controller.Conflict(localizer["MaterialCatalogDuplicated"].Value),

                    UpdatePartialMaterialCatalogError.UnexpectedError =>
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
}

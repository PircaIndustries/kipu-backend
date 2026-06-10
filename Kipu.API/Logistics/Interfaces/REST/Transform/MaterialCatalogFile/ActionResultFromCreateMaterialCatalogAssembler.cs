using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MaterialCatalogEntity = Kipu.API.Logistics.Domain.Model.Aggregates.MaterialCatalog;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCatalog;

public static class ActionResultFromCreateMaterialCatalogAssembler
{
    public static ActionResult ToActionResultFromCreateMaterialCatalogResult(
        Result<MaterialCatalogEntity, CreateMaterialCatalogError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getMaterialCatalogByIdActionName) =>
        result switch
        {
            Result<MaterialCatalogEntity, CreateMaterialCatalogError>.Success success =>
                controller.CreatedAtAction(getMaterialCatalogByIdActionName, new { id = success.Value.Id },
                    MaterialCatalogResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<MaterialCatalogEntity, CreateMaterialCatalogError>.Failure failure =>
                failure.Error switch
                {
                    CreateMaterialCatalogError.DuplicatedMaterialCatalog =>
                        controller.Conflict(localizer["MaterialCatalogDuplicated"].Value),

                    CreateMaterialCatalogError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingMaterialCatalog"].Value,
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


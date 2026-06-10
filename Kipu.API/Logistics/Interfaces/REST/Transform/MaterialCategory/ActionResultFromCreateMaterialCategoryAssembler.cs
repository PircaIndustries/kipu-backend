using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MaterialCategoryEntity = Kipu.API.Logistics.Domain.Model.Aggregates.MaterialCategory;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCategory;

public static class ActionResultFromCreateMaterialCategoryAssembler
{
    public static ActionResult ToActionResultFromCreateMaterialCategoryResult(
        Result<MaterialCategoryEntity, CreateMaterialCategoryError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getMaterialCategoryByIdActionName) =>
        result switch
        {
            Result<MaterialCategoryEntity, CreateMaterialCategoryError>.Success success =>
                controller.CreatedAtAction(getMaterialCategoryByIdActionName, new { id = success.Value.Id },
                    MaterialCategoryResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<MaterialCategoryEntity, CreateMaterialCategoryError>.Failure failure =>
                failure.Error switch
                {
                    CreateMaterialCategoryError.DuplicatedMaterialCategory =>
                        controller.Conflict(localizer["MaterialCategoryDuplicated"].Value),

                    CreateMaterialCategoryError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingMaterialCategory"].Value,
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
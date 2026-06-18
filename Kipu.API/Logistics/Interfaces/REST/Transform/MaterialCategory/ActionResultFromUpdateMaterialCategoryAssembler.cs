using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MaterialCategoryEntity = Kipu.API.Logistics.Domain.Model.Aggregates.MaterialCategory;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.MaterialCategory;

public static class ActionResultFromUpdateMaterialCategoryAssembler
{
    public static ActionResult ToActionResultFromUpdateMaterialCategoryResult(
        Result<MaterialCategoryEntity, UpdateMaterialCategoryError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<MaterialCategoryEntity, UpdateMaterialCategoryError>.Success success =>
                controller.Ok(MaterialCategoryResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<MaterialCategoryEntity, UpdateMaterialCategoryError>.Failure failure =>
                failure.Error switch
                {
                    UpdateMaterialCategoryError.MaterialCategoryNotFound =>
                        controller.NotFound(localizer["MaterialCategoryNotFound"].Value),

                    UpdateMaterialCategoryError.DuplicatedMaterialCategory =>
                        controller.Conflict(localizer["MaterialCategoryDuplicated"].Value),

                    UpdateMaterialCategoryError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingMaterialCategory"].Value,
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

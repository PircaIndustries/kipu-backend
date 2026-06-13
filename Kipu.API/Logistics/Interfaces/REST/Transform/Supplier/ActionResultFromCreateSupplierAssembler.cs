using Kipu.API.Logistics.Application.Errors;
using SupplierEntity = Kipu.API.Logistics.Domain.Model.Aggregates.Supplier;
using Kipu.API.Logistics.Interfaces.REST.Resources.Supplier;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Kipu.API.Resources;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.Supplier;

public static class ActionResultFromCreateSupplierAssembler
{
    public static ActionResult ToActionResultFromCreateSupplierResult(
        Result<SupplierEntity, CreateSupplierError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getSupplierByIdActionName) =>
        result switch
        {
            Result<SupplierEntity, CreateSupplierError>.Success success =>
                controller.CreatedAtAction(
                    getSupplierByIdActionName, 
                    new { id = success.Value.Id },
                    SupplierResourceFromEntityAssembler.ToResourceFromEntity(success.Value)
                ),

            Result<SupplierEntity, CreateSupplierError>.Failure failure =>
                failure.Error switch
                {
                    CreateSupplierError.DuplicatedSupplier =>
                        controller.Conflict(localizer["SupplierDuplicated"].Value),

                    CreateSupplierError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingSupplier"].Value,
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
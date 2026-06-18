using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SupplierOfferEntity = Kipu.API.Logistics.Domain.Model.Aggregates.SupplierOffer;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.SupplierOffer;

public static class ActionResultFromCreateSupplierOfferAssembler
{
    public static ActionResult ToActionResultFromCreateSupplierOfferResult(
        Result<SupplierOfferEntity, CreateSupplierOfferError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getByIdActionName) =>
        result switch
        {
            Result<SupplierOfferEntity, CreateSupplierOfferError>.Success success =>
                controller.CreatedAtAction(getByIdActionName, new { id = success.Value.Id },
                    SupplierOfferResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),
            Result<SupplierOfferEntity, CreateSupplierOfferError>.Failure failure =>
                failure.Error switch
                {
                    CreateSupplierOfferError.DuplicatedSupplierOffer =>
                        controller.Conflict(localizer["SupplierOfferDuplicated"].Value),
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

using Kipu.API.Logistics.Application.Errors;
using Kipu.API.Resources;
using Kipu.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SupplierOfferEntity = Kipu.API.Logistics.Domain.Model.Aggregates.SupplierOffer;

namespace Kipu.API.Logistics.Interfaces.REST.Transform.SupplierOffer;

public static class ActionResultFromUpdateSupplierOfferAssembler
{
    public static ActionResult ToActionResultFromUpdateSupplierOfferResult(
        Result<SupplierOfferEntity, UpdateSupplierOfferError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<SupplierOfferEntity, UpdateSupplierOfferError>.Success success =>
                controller.Ok(SupplierOfferResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),
            Result<SupplierOfferEntity, UpdateSupplierOfferError>.Failure failure =>
                failure.Error switch
                {
                    UpdateSupplierOfferError.SupplierOfferNotFound =>
                        controller.NotFound(localizer["SupplierOfferNotFound"].Value),
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

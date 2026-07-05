using System.Net.Mime;
using Kipu.API.Logistics.MaterialWaste.Application.Services;
using Kipu.API.Logistics.MaterialWaste.Domain.Model.Queries;
using Kipu.API.Logistics.MaterialWaste.Interface.REST.Resources;
using Kipu.API.Logistics.MaterialWaste.Interface.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Kipu.API.Logistics.MaterialWaste.Interface.REST.Controllers;

[ApiController]
[Route("api/v1/wasteClassifications")]
[Produces(MediaTypeNames.Application.Json)]
public class WasteClassificationsController(
    IWasteClassificationCommandService commandService,
    IWasteClassificationQueryService queryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WasteClassificationResource>>> GetWasteClassifications()
    {
        var query = new GetAllWasteClassificationsQuery();
        var result = await queryService.Handle(query);
        var resources = result.Select(WasteClassificationAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPost]
    public async Task<ActionResult<WasteClassificationResource>> CreateWasteClassification([FromBody] CreateWasteClassificationResource resource)
    {
        var command = WasteClassificationAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command);
        var response = WasteClassificationAssembler.ToResourceFromEntity(result);
        return Created("", response);
    }
}
using System.Net.Mime;
using Kipu.API.Logistics.MaterialWaste.Application.Services;
using Kipu.API.Logistics.MaterialWaste.Domain.Model.Commands;
using Kipu.API.Logistics.MaterialWaste.Domain.Model.Queries;
using Kipu.API.Logistics.MaterialWaste.Interface.REST.Resources;
using Kipu.API.Logistics.MaterialWaste.Interface.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Kipu.API.Logistics.MaterialWaste.Interface.REST.Controllers;

[ApiController]
[Route("api/v1/materials/waste")]
[Produces(MediaTypeNames.Application.Json)]
public class MaterialsWasteController(
    IMaterialWasteCommandService commandService,
    IMaterialWasteQueryService queryService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<MaterialWasteResource>> CreateMaterialWaste([FromBody] CreateMaterialWasteResource resource)
    {
        var command = MaterialWasteAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command);
        var response = MaterialWasteAssembler.ToResourceFromEntity(result);
        return CreatedAtAction(nameof(GetMaterialWasteById), new { id = response.Id }, response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaterialWasteResource>>> GetMaterialWastes()
    {
        var query = new GetAllMaterialWastesQuery();
        var result = await queryService.Handle(query);
        var resources = result.Select(MaterialWasteAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MaterialWasteResource>> GetMaterialWasteById(string id)
    {
        var query = new GetMaterialWasteByIdQuery(id);
        var result = await queryService.Handle(query);
        if (result is null) return NotFound();
        return Ok(MaterialWasteAssembler.ToResourceFromEntity(result));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MaterialWasteResource>> UpdateMaterialWaste(string id, [FromBody] UpdateMaterialWasteResource resource)
    {
        var command = MaterialWasteAssembler.ToCommandFromResource(id, resource);
        var result = await commandService.Handle(command);
        return Ok(MaterialWasteAssembler.ToResourceFromEntity(result));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialWaste(string id)
    {
        var command = new DeleteMaterialWasteCommand(id);
        await commandService.Handle(command);
        return NoContent();
    }
}
using System.Net.Mime;
using Kipu.API.Ncr.Application.Internal.CommandServices;
using Kipu.API.NCR.Domain.Model.Aggregates;
using Kipu.API.NCR.Domain.Repositories;
using Kipu.API.Ncr.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Kipu.API.Ncr.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class NcrsController : ControllerBase
{
    private readonly NcrCommandService _ncrCommandService;
    private readonly INcrRepository _ncrRepository;

    public NcrsController(NcrCommandService ncrCommandService, INcrRepository ncrRepository)
    {
        _ncrCommandService = ncrCommandService;
        _ncrRepository = ncrRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNcr([FromBody] CreateNcrResource resource)
    {
        var result = await _ncrCommandService.HandleCreateAsync(
            resource.Title, resource.Description, resource.TaskName, resource.Severity, resource.ProjectId);

        return result.Fold<IActionResult>(
            onSuccess: ncr => CreatedAtAction(nameof(GetNcrById), new { id = ncr.Id }, MapToResource(ncr)),
            onFailure: error => BadRequest(new { message = error })
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNcrs()
    {
        var ncrs = await _ncrRepository.ListAsync();
        var resources = ncrs.Select(MapToResource);
        return Ok(resources);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetNcrById(int id)
    {
        var ncr = await _ncrRepository.FindByIdAsync(id);
        if (ncr == null) return NotFound();
        return Ok(MapToResource(ncr));
    }

    private static NcrResource MapToResource(NCR.Domain.Model.Aggregates.Ncr ncr)
    {
        return new NcrResource(
            ncr.Id, ncr.Title, ncr.Description, ncr.TaskName, 
            ncr.Severity.ToString(), ncr.ProjectId, ncr.Status.ToString(), 
            ncr.RootCause, ncr.CorrectiveAction);
    }
}
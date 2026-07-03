using Kipu.API.Document.Application.Services;
using Kipu.API.Document.Domain.Model.Commands;
using Kipu.API.Document.Domain.Model.Queries;
using Kipu.API.Document.Interfaces.REST.Resources;
using Kipu.API.Document.Interfaces.REST.Transform;
using Kipu.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Kipu.API.Document.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentCommandService _documentCommandService;
    private readonly IDocumentQueryService _documentQueryService;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public DocumentsController(
        IDocumentCommandService documentCommandService, 
        IDocumentQueryService documentQueryService,
        IStringLocalizer<SharedResource> localizer)
    {
        _documentCommandService = documentCommandService;
        _documentQueryService = documentQueryService;
        _localizer = localizer;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentResource resource) 
    {
        var command = CreateDocumentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _documentCommandService.Handle(command);
        
        if (result is null)
            return BadRequest(new { message = _localizer["DocumentNotCreated"].Value });
        
        var responseResource = DocumentResourceFromEntityAssembler.ToResourceFromEntity(result);
        return CreatedAtAction(nameof(GetDocumentById), new { documentId = result.Id.Value }, responseResource);
    }
    
    [HttpPost("{documentId}/send-code")]
    public async Task<IActionResult> SendSignCode(string documentId, [FromBody] SendSignCodeRequest request)
    {
        var command = new SendSignCodeCommand(documentId, request.Email);
        var success = await _documentCommandService.Handle(command);

        if (!success)
            return BadRequest(new { message = _localizer["DocumentSignFailed"].Value });

        return Ok(new { message = "Código enviado al correo." });
    }

    [HttpPost("{documentId}/sign")]
    public async Task<IActionResult> SignDocument(string documentId, [FromBody] SignDocumentRequest request)
    {
        var command = new SignDocumentAsParticipantCommand(documentId, request.TeamUserId, request.Code, request.Email);
        var result = await _documentCommandService.Handle(command);

        if (result is null)
            return BadRequest(new { message = "Código de verificación incorrecto o expirado." });

        var resource = DocumentResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
    
    [HttpGet("{documentId}")]
    public async Task<IActionResult> GetDocumentById(string documentId)
    {
        var query = new GetDocumentByIdQuery(documentId);
        var result = await _documentQueryService.Handle(query);

        if (result is null)
            return NotFound(new { message = _localizer["DocumentNotFound"].Value });

        var resource = DocumentResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
    
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingDocuments([FromQuery] string projectId, [FromQuery] string teamUserId)
    {
        var query = new GetPendingDocumentsForUserQuery(projectId, teamUserId);
        var results = await _documentQueryService.Handle(query);

        var resources = results.Select(DocumentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpGet("signed")]
    public async Task<IActionResult> GetSignedDocuments([FromQuery] string projectId, [FromQuery] string teamUserId)
    {
        var query = new GetSignedDocumentsForUserQuery(projectId, teamUserId);
        var results = await _documentQueryService.Handle(query);

        var resources = results.Select(DocumentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}
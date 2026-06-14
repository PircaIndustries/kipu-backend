using Kipu.API.Document.Application.Services;
using Kipu.API.Document.Domain.Model.Queries;
using Kipu.API.Document.Domain.Model.ValueObjects;
using Kipu.API.Document.Domain.Repositories;

namespace Kipu.API.Document.Application.Internal.QueryServices;

public class DocumentQueryService : IDocumentQueryService
{
    private readonly IDocumentRepository _documentRepository;

    public DocumentQueryService(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<Domain.Model.Aggregates.Document?> Handle(GetDocumentByIdQuery query)
    {
        var documentId = new DocumentId(query.DocumentId);
        return await _documentRepository.FindByIdAsync(documentId);
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.Document>> Handle(GetPendingDocumentsForUserQuery query)
    {
        return await _documentRepository.FindPendingByProjectIdAndUserIdAsync(query.ProjectId, query.TeamUserId);
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.Document>> Handle(GetSignedDocumentsForUserQuery query)
    {
        return await _documentRepository.FindSignedByProjectIdAndUserIdAsync(query.ProjectId, query.TeamUserId);
    }
}
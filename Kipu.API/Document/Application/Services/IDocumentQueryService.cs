using Kipu.API.Document.Domain.Model.Queries;

namespace Kipu.API.Document.Application.Services;

public interface IDocumentQueryService
{
    Task<Domain.Model.Aggregates.Document?> Handle(GetDocumentByIdQuery query);
    Task<IEnumerable<Domain.Model.Aggregates.Document>> Handle(GetPendingDocumentsForUserQuery query);
    Task<IEnumerable<Domain.Model.Aggregates.Document>> Handle(GetSignedDocumentsForUserQuery query);
}
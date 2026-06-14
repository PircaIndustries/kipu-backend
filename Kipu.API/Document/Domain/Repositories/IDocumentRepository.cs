using Kipu.API.Document.Domain.Model.ValueObjects;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Document.Domain.Repositories;

public interface IDocumentRepository : IBaseAggregateRepository<Model.Aggregates.Document, DocumentId>
{
    Task<IEnumerable<Model.Aggregates.Document>> FindPendingByProjectIdAndUserIdAsync(string projectId, string teamUserId);
    Task<IEnumerable<Model.Aggregates.Document>> FindSignedByProjectIdAndUserIdAsync(string projectId, string teamUserId);
}
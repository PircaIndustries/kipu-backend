using Kipu.API.Document.Domain.Model.ValueObjects;
using Kipu.API.Document.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kipu.API.Document.Infraestructure.Persistence.EFC.Repositories;

public class DocumentRepository : BaseAggregateRepository<Domain.Model.Aggregates.Document, DocumentId>, IDocumentRepository
{
    public DocumentRepository(AppDbContext context) : base(context)
    {
    }

    public new async Task<Domain.Model.Aggregates.Document?> FindById(DocumentId id)
    {
        return await Context.Set<Domain.Model.Aggregates.Document>()
            .Include(d => d.Participants) 
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.Document>> FindPendingByProjectIdAndUserIdAsync(string projectId, string teamUserId)
    {
        return await Context.Set<Domain.Model.Aggregates.Document>()
            .Include(d => d.Participants)
            .Where(d => 
                d.ProjectId == projectId && 
                d.Participants.Any(p => p.TeamUserId == teamUserId && p.SignedAt == null)
            )
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.Document>> FindSignedByProjectIdAndUserIdAsync(string projectId, string teamUserId)
    {
        return await Context.Set<Domain.Model.Aggregates.Document>()
            .Include(d => d.Participants)
            .Where(d => 
                d.ProjectId == projectId && 
                d.Participants.Any(p => p.TeamUserId == teamUserId && p.SignedAt != null)
            )
            .ToListAsync();
    }
}
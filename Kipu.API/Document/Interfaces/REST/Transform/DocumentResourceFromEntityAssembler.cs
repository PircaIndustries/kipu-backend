using Kipu.API.Document.Interfaces.REST.Resources;

namespace Kipu.API.Document.Interfaces.REST.Transform;

public static class DocumentResourceFromEntityAssembler
{
    public static DocumentResource ToResourceFromEntity(Domain.Model.Aggregates.Document entity)
    {
        var participants = entity.Participants.Select(p => new DocumentParticipantResource(
            p.TeamUserId,
            p.FullName
        )).ToList();

        return new DocumentResource(
            entity.Id.Value,
            entity.Type,
            entity.IsSigned,
            entity.DigitalSignatureToken,
            entity.Deadline,
            entity.ProjectId,
            participants
        );
    }
}
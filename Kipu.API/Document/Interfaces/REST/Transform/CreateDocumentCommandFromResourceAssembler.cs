using Kipu.API.Document.Domain.Model.Commands;
using Kipu.API.Document.Interfaces.REST.Resources;

namespace Kipu.API.Document.Interfaces.REST.Transform;

public static class CreateDocumentCommandFromResourceAssembler
{
    public static CreateDocumentCommand ToCommandFromResource(CreateDocumentResource resource)
    {
        var participants = resource.Participants.Select(p => 
            new DocumentParticipantItem(p.TeamUserId, p.FullName)
        ).ToList();

        return new CreateDocumentCommand(
            resource.Type,
            resource.Deadline,
            resource.ProjectId,
            participants
        );
    }
}
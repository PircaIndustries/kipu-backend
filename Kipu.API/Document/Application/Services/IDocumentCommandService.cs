using Kipu.API.Document.Domain.Model.Commands;

namespace Kipu.API.Document.Application.Services;

public interface IDocumentCommandService
{
    Task<Domain.Model.Aggregates.Document?> Handle(CreateDocumentCommand command);
    Task<Domain.Model.Aggregates.Document?> Handle(SignDocumentAsParticipantCommand command);
}
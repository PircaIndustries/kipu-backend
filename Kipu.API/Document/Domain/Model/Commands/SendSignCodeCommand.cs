namespace Kipu.API.Document.Domain.Model.Commands;

public record SendSignCodeCommand(string DocumentId, string Email);

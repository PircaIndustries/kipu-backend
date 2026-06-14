namespace Kipu.API.Document.Domain.Model.Queries;

public record GetSignedDocumentsForUserQuery(string ProjectId, string TeamUserId);
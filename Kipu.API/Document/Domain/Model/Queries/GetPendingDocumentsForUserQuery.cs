namespace Kipu.API.Document.Domain.Model.Queries;

public record GetPendingDocumentsForUserQuery(string ProjectId, string TeamUserId);
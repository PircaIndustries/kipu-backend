namespace Kipu.API.IAM.Interfaces.REST.Resources;

public record AuthenticatedUserResource(int Id, string Name, string Email, string Role, string Token);

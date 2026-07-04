namespace Kipu.API.IAM.Interfaces.REST.Resources;

public record VerifyLoginResource(string Email, string Code, bool RememberMe = false);

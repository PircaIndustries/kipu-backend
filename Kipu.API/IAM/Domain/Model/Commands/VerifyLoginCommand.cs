namespace Kipu.API.IAM.Domain.Model.Commands;

public record VerifyLoginCommand(string Email, string Code, bool RememberMe);

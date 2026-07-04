namespace Kipu.API.IAM.Interfaces.REST.Resources;

public record ResetPasswordResource(string Email, string Code, string NewPassword);

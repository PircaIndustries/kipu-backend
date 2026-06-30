namespace Kipu.API.IAM.Domain.Model.Commands;

public record ResetPasswordCommand(string Email, string Code, string NewPassword);

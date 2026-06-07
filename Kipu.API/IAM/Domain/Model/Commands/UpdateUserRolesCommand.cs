namespace Kipu.API.IAM.Domain.Model.Commands;

public record UpdateUserRolesCommand(int UserId, List<string> Roles);

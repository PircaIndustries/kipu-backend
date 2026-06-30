using Kipu.API.IAM.Domain.Model.Aggregates;
using Kipu.API.IAM.Domain.Model.Commands;
using Kipu.API.Shared.Application.Patterns;

namespace Kipu.API.IAM.Application.Services;

public interface IUserCommandService
{
    Task<Result<User, string>> Handle(SignUpCommand command);
    Task<Result<(User User, string Token), string>> Handle(SignInCommand command);
    Task<Result<User, string>> Handle(UpdateUserRolesCommand command);
    Task<Result<(User User, string Token), string>> Handle(VerifyLoginCommand command);
    Task<Result<string, string>> Handle(ForgotPasswordCommand command);
    Task<Result<string, string>> Handle(ResetPasswordCommand command);
}

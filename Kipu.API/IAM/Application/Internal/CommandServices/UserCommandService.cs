using Kipu.API.IAM.Application.Services;
using Kipu.API.IAM.Domain.Model.Aggregates;
using Kipu.API.IAM.Domain.Model.Commands;
using Kipu.API.IAM.Domain.Model.ValueObjects;
using Kipu.API.IAM.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.IAM.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    IHashingService hashingService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork) : IUserCommandService
{
    public async Task<Result<User, string>> Handle(SignUpCommand command)
    {
        if (await userRepository.ExistsByEmailAsync(command.Email))
        {
            return new Result<User, string>.Failure("EmailAlreadyRegistered");
        }

        if (!Roles.IsValid(command.Role))
        {
            return new Result<User, string>.Failure("InvalidRole");
        }

        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new User(command.Name, command.Email, hashedPassword, command.Role);

        try
        {
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
            return new Result<User, string>.Success(user);
        }
        catch (Exception ex)
        {
            return new Result<User, string>.Failure(ex.Message);
        }
    }

    public async Task<Result<(User User, string Token), string>> Handle(SignInCommand command)
    {
        var user = await userRepository.FindByEmailAsync(command.Email);
        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
        {
            return new Result<(User User, string Token), string>.Failure("InvalidCredentials");
        }

        var token = tokenService.GenerateToken(user);
        return new Result<(User User, string Token), string>.Success((user, token));
    }

    public async Task<Result<User, string>> Handle(UpdateUserRolesCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId);
        if (user == null)
        {
            return new Result<User, string>.Failure("UserNotFound");
        }

        foreach (var role in command.Roles)
        {
            if (!Roles.IsValid(role))
            {
                return new Result<User, string>.Failure("InvalidRole");
            }
        }

        if (command.Roles.Count == 0)
        {
            return new Result<User, string>.Failure("RoleRequired");
        }

        user.Role = command.Roles[0]; // Assuming single role per user based on DB schema

        try
        {
            userRepository.Update(user);
            await unitOfWork.CompleteAsync();
            return new Result<User, string>.Success(user);
        }
        catch (Exception ex)
        {
            return new Result<User, string>.Failure(ex.Message);
        }
    }
}

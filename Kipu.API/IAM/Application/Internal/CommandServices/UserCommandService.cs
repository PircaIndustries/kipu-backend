using Kipu.API.IAM.Application.Services;
using Kipu.API.IAM.Domain.Model.Aggregates;
using Kipu.API.IAM.Domain.Model.Commands;
using Kipu.API.IAM.Domain.Model.ValueObjects;
using Kipu.API.IAM.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;
using Kipu.API.Shared.Domain.Services;

namespace Kipu.API.IAM.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    IHashingService hashingService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    IOtpService otpService,
    IEmailService emailService) : IUserCommandService
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

        var otp = otpService.GenerateOtp(user.Email, "Login");
        await emailService.SendEmailAsync(user.Email, "Código de Verificación", $"Tu código de inicio de sesión es: {otp}. Es válido por 15 minutos.");

        return new Result<(User User, string Token), string>.Success((user, "OTP_SENT"));
    }

    public async Task<Result<(User User, string Token), string>> Handle(VerifyLoginCommand command)
    {
        var user = await userRepository.FindByEmailAsync(command.Email);
        if (user == null)
        {
            return new Result<(User User, string Token), string>.Failure("UserNotFound");
        }

        if (!otpService.ValidateOtp(command.Email, "Login", command.Code))
        {
            return new Result<(User User, string Token), string>.Failure("InvalidOtp");
        }

        var token = tokenService.GenerateToken(user, command.RememberMe);
        return new Result<(User User, string Token), string>.Success((user, token));
    }

    public async Task<Result<string, string>> Handle(ForgotPasswordCommand command)
    {
        var user = await userRepository.FindByEmailAsync(command.Email);
        if (user == null)
        {
            // Do not reveal if user exists or not, just return success
            return new Result<string, string>.Success("EmailSent");
        }

        var otp = otpService.GenerateOtp(user.Email, "ResetPassword");
        await emailService.SendEmailAsync(user.Email, "Recuperación de Contraseña", $"Tu código para restablecer la contraseña es: {otp}. Es válido por 15 minutos.");

        return new Result<string, string>.Success("EmailSent");
    }

    public async Task<Result<string, string>> Handle(ResetPasswordCommand command)
    {
        var user = await userRepository.FindByEmailAsync(command.Email);
        if (user == null)
        {
            return new Result<string, string>.Failure("UserNotFound");
        }

        if (!otpService.ValidateOtp(command.Email, "ResetPassword", command.Code))
        {
            return new Result<string, string>.Failure("InvalidOtp");
        }

        user.PasswordHash = hashingService.HashPassword(command.NewPassword);
        userRepository.Update(user);
        await unitOfWork.CompleteAsync();

        await emailService.SendEmailAsync(user.Email, "Contraseña Cambiada", "Tu contraseña ha sido cambiada exitosamente.");

        return new Result<string, string>.Success("PasswordResetSuccess");
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

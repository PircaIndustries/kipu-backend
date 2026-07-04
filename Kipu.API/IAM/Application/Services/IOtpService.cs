namespace Kipu.API.IAM.Application.Services;

public interface IOtpService
{
    string GenerateOtp(string email, string purpose);
    bool ValidateOtp(string email, string purpose, string code);
}

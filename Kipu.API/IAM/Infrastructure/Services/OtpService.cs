using Kipu.API.IAM.Application.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Kipu.API.IAM.Infrastructure.Services;

public class OtpService(IMemoryCache memoryCache) : IOtpService
{
    private static readonly TimeSpan OtpExpiration = TimeSpan.FromMinutes(15);

    public string GenerateOtp(string email, string purpose)
    {
        var random = new Random();
        var otp = random.Next(100000, 999999).ToString();
        var cacheKey = GetCacheKey(email, purpose);

        memoryCache.Set(cacheKey, otp, OtpExpiration);

        return otp;
    }

    public bool ValidateOtp(string email, string purpose, string code)
    {
        var cacheKey = GetCacheKey(email, purpose);

        if (memoryCache.TryGetValue(cacheKey, out string? storedOtp))
        {
            if (storedOtp == code)
            {
                // Remove OTP after successful validation
                memoryCache.Remove(cacheKey);
                return true;
            }
        }

        return false;
    }

    private static string GetCacheKey(string email, string purpose) => $"OTP_{purpose}_{email}";
}

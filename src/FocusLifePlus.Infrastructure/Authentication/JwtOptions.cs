namespace FocusLifePlus.Infrastructure.Authentication;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public int TokenExpirationInMinutes { get; set; }
    public int RefreshTokenTTL { get; set; }
} 
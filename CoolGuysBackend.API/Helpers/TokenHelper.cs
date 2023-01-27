using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CoolGuysBackend.Helpers;

public class TokenHelper
{
    private readonly string secret;

    public TokenHelper(string secret)
    {
        this.secret = secret;
    }

    public int GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var userId = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        // validate the token
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "coolguys",
            ValidAudience = "coolguys",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };

        try
        {
            handler.ValidateToken(token, validationParameters, out var validatedToken);
        }
        catch (SecurityTokenException)
        {
            // the token is invalid
            return -1;
        }

        return int.Parse(userId);
    }
}
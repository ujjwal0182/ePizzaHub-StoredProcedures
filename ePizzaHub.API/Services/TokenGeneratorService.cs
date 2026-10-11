using ePizzaHub.API.Settings;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Response;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ePizzaHub.API.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly JwtSettings _jwt;
        public TokenGeneratorService(IOptions<JwtSettings> options) => _jwt = options.Value;

        public string GenerateToken(ValidateUserResponse user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email),
                new("UserId", user.UserId.ToString()),                         // UI isi ko padhta hai
                new("IsAdmin", user.Roles.Contains("Admin").ToString()),
                new("Roles", JsonSerializer.Serialize(user.Roles))
            };
            claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r))); // [Authorize(Roles="Admin")] ke liye

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwt.TokenExpiryInMinutes),
                Issuer = _jwt.Issuer,
                Audience = _jwt.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            return new JsonWebTokenHandler().CreateToken(descriptor);
        }
    }
}

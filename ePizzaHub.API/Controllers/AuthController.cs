using ePizzaHub.API.Settings;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Models.ApiModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenGeneratorService _tokenGenerator;
        private readonly JwtSettings _jwt;

        public AuthController(IAuthService authService, ITokenGeneratorService tokenGenerator, IOptions<JwtSettings> jwt)
        {
            _authService = authService;
            _tokenGenerator = tokenGenerator;
            _jwt = jwt.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.ValidateUserAsync(request.Email, request.Password);
            return Ok(new AuthAPIResponse
            {
                AccessToken = _tokenGenerator.GenerateToken(user),
                TokenExpiryInMinutes = _jwt.TokenExpiryInMinutes
            });
        }

        // Sirf token test karne ke liye
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me() => Ok(User.Claims.Select(c => new { c.Type, c.Value }));
    
    }
}

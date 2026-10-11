using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net;
using ePizzaHub.UI.Helpers;
using ePizzaHub.UI.Models.ApiModels.Request;
using ePizzaHub.UI.Models.ApiModels.Response;
using ePizzaHub.UI.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenService _tokenService;

        public LoginController(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel request)
        {
            if (!ModelState.IsValid) return View(request);

            var client = _httpClientFactory.CreateClient("ePizzaAPI");
            var response = await client.PostAsJsonAsync("Auth/login",
                new { Email = request.EmailAddress, Password = request.Password });

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(request);
            }
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, await response.ReadErrorAsync());
                return View(request);
            }

            var auth = (await response.Content.ReadFromJsonAsync<ValidateUserResponse>())!;
            _tokenService.SetToken(auth.AccessToken);   // API ke liye: HttpOnly cookie me JWT

            // MVC ke liye: JWT ke claims se cookie-login (BaseController.CurrentUser inhi se padhta hai)
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(auth.AccessToken);
            await SignInAsync(jwt.Claims.ToList());

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("AccessToken");
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel request)
        {
            if (!ModelState.IsValid) return View(request);

            var client = _httpClientFactory.CreateClient("ePizzaAPI");
            var response = await client.PostAsJsonAsync("User", new CreateUserRequestModel
            {
                Name = request.UserName,
                Email = request.Email,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber
            });

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                ModelState.AddModelError(nameof(request.Email), "This email is already registered.");
                return View(request);
            }
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, await response.ReadErrorAsync());
                return View(request);
            }

            return RedirectToAction("Login");   // pehle yahan wapas Register page hi khulta tha
        }

        private async Task SignInAsync(List<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { IsPersistent = false, ExpiresUtc = DateTime.UtcNow.AddMinutes(60) });
        }
    }
}
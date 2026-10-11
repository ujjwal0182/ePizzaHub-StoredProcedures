namespace ePizzaHub.UI.Helpers
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string GetToken()
        {
            if(httpContextAccessor.HttpContext != null)
            {
                return httpContextAccessor.HttpContext.Request.Cookies["AccessToken"] ?? string.Empty;
            }
            throw new Exception("AccessToken cookie not found in the current HTTP context.");
        }

        public void SetToken(string token)
        {
            if(httpContextAccessor.HttpContext != null)
            {
                httpContextAccessor.HttpContext.Response.Cookies.Append("AccessToken", token, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddMinutes(60), // Set the cookie to expire in 7 days
                    HttpOnly = true, // Make the cookie accessible only through HTTP requests
                    Secure = true, // Ensure the cookie is sent over HTTPS
                });
            }
        }
    }
}

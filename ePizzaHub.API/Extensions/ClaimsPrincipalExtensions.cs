
using System.Security.Claims;

/// Get UserId from Token
namespace ePizzaHub.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        // Login nahi hai (guest) to 0
        public static int GetUserId(this ClaimsPrincipal user) =>
            int.TryParse(user.FindFirstValue("UserId"), out var id) ? id : 0;
    }
}

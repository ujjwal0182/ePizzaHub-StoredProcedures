using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Controllers
{
    [Authorize]
    [Route("Assistant")]
    public class AssistantController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AssistantController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        // CartController wali hi logic: cookie se lo, na ho to nayi banao
        private Guid ResolveCartId()
        {
            if (Guid.TryParse(Request.Cookies["CartId"], out var existing)) return existing;

            var id = Guid.NewGuid();
            Response.Cookies.Append("CartId", id.ToString(), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(2),
                HttpOnly = true,
                Secure = true
            });
            return id;
        }

        [HttpPost("Chat")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Chat([FromBody] AssistantChatInput input, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(input?.Message) || input.Message.Length > 500)
                return BadRequest(new { reply = "Message 1 se 500 characters ke beech hona chahiye." });

            var cartId = ResolveCartId();

            var client = _httpClientFactory.CreateClient("ePizzaAPI");
            client.Timeout = TimeSpan.FromMinutes(5);   // default 100s hai, local LLM ke liye kam padega

            var res = await client.PostAsJsonAsync("Assistant/chat",
                new { Message = input.Message.Trim(), CartId = cartId.ToString() }, ct);

            if (res.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(new { reply = "Session expire ho gaya, dobara login karo." });
            if (!res.IsSuccessStatusCode)
                return StatusCode(502, new { reply = "Assistant abhi available nahi hai. Thodi der baad try karo." });

            var body = await res.Content.ReadFromJsonAsync<AssistantChatReply>(cancellationToken: ct);
            return Ok(body);
        }
    }

    public record AssistantChatInput(string Message);
    public record AssistantChatReply(string Reply);
}
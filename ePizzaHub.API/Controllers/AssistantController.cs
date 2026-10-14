using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ePizzaHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssistantController : ControllerBase
    {
        private readonly IAssistantService _assistantService;
        public AssistantController(IAssistantService assistantService)
        {
            _assistantService = assistantService;
        }

        [HttpPost("chat")]
        [EnableRateLimiting("assistant")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest req, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.Message) || req.Message.Length > 500)
                return BadRequest("Message is required (max 500 characters).");

            var bearer = Request.Headers.Authorization.ToString();
            var reply = await _assistantService.ChatAsync(req.Message.Trim(), req.CartId, bearer, ct);
            return Ok(new { reply });
        }
    }
}

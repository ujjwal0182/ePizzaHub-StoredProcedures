using ePizzaHub.API.Extensions;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Models.ApiModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request.CartId == Guid.Empty) return BadRequest("Cart Id cannot be empty.");
            return Ok(await _paymentService.CreateOrderAsync(request.CartId, User.GetUserId()));
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody] MakePaymentRequest request)
        {
            if (request.CartId == Guid.Empty) return BadRequest("Cart Id cannot be empty.");

            var userId = User.GetUserId();
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

            var orderId = await _paymentService.MakePaymentAsync(request, userId, email);
            return Ok(new MakePaymentResponse { OrderId = orderId });
        }
    }
}

using ePizzaHub.API.Extensions;
using ePizzaHub.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService) => _orderService = orderService;

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
            => Ok(await _orderService.GetOrdersAsync(User.GetUserId()));
    }
}

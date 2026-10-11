using ePizzaHub.API.Extensions;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService) => _cartService = cartService;

        [HttpGet("get-item-count")]
        [AllowAnonymous]
        public async Task<IActionResult> GetItemCount(Guid guid)
        {
            if (guid == Guid.Empty) return BadRequest("Cart Id can not be empty.");
            return Ok(await _cartService.GetItemCountAsync(guid));
        }

        [HttpGet("get-cart-details")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCartDetails(Guid cartId)
        {
            if (cartId == Guid.Empty) return BadRequest("Cart Id cannot be empty.");
            return Ok(await _cartService.GetCartDetailsAsync(cartId));
        }

        [HttpPost("add-item-to-cart")]
        [AllowAnonymous]
        public async Task<IActionResult> AddItemToCart(AddToCartRequest request)
        {
            if (request.cartId == Guid.Empty) return BadRequest("Cart Id cannot be empty.");

            // Client ka bheja UserId kabhi nahi. Token hai to uska, warna guest = 0
            request.UserId = User.GetUserId();
            return Ok(await _cartService.AddItemAsync(request));
        }

        [HttpPut("update-item")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateItem(UpdateCartItemRequest request)
        {
            if(request.CartId == Guid.Empty) return BadRequest("Cart Id cannot be empty.");
            return Ok(await _cartService.UpdateItemQuantityAsync(request.CartId, request.ItemId,request.Quantity));
        }

        [HttpDelete("delete-item")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteItem(Guid cartId, int itemId)
        {
            if (cartId == Guid.Empty) return BadRequest("Cart Id cannot be empty.");
            var removed = await _cartService.RemoveItemAsync(cartId, itemId);
            return removed ? Ok(true) : NotFound();
        }

        [HttpPut("update-cart-user")]
        public async Task<IActionResult> UpdateCartUser(UpdateCartUserRequest request)
        {
            if (request.CartId == Guid.Empty) return BadRequest("Cart Id cannot be empty.");

            // Body ka UserId ignore, nahi to koi apna cart kisi aur ke naam kar de
            await _cartService.UpdateCartUserAsync(request.CartId, User.GetUserId());
            return Ok(true);
        }
    }
}

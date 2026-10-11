using System.Net;
using ePizzaHub.UI.Helpers;
using ePizzaHub.UI.Models.ApiModels.Response;
using ePizzaHub.UI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Controllers
{
    [Route("Cart")]
    public class CartController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CartController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        // Bug fix: ek request me ek hi GUID. Pehle har access par nayi GUID ban sakti thi.
        private Guid? _cartId;
        private Guid CartId => _cartId ??= ResolveCartId();

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

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("ePizzaAPI");
            var cart = await client.GetFromJsonAsync<GetCartResponseModel>($"Cart/get-cart-details?cartId={CartId}");
            return View(cart);
        }

        [HttpPost("AddToCart/{itemId:int}/{quantity:int}")]
        public async Task<IActionResult> AddToCart(int itemId, int quantity)
        {
            var client = _httpClientFactory.CreateClient("ePizzaAPI");
            var response = await client.PostAsJsonAsync("Cart/add-item-to-cart",
                new { ItemId = itemId, Quantity = quantity, CartId });   // UserId API token se leti hai

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, new { Message = await response.ReadErrorAsync() });

            return Json(new { Count = await CartItemsCount() });
        }

        [HttpGet("GetCartItemCount")]
        public async Task<JsonResult> GetCartItemCount() => Json(new { Count = await CartItemsCount() });

        [HttpPut("UpdateQuantity/{itemId:int}/{quantity:int}")]
        public async Task<IActionResult> UpdateQuantity(int itemId, int quantity)
        {
            var client = _httpClientFactory.CreateClient("ePizzaAPI");

            // quantity 0 ka matlab "hata do" (API quantity >= 1 hi leti hai)
            var response = quantity <= 0
                ? await client.DeleteAsync($"Cart/delete-item?cartId={CartId}&itemId={itemId}")
                : await client.PutAsJsonAsync("Cart/update-item", new { CartId, ItemId = itemId, Quantity = quantity });

            if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                return StatusCode((int)response.StatusCode, new { Message = await response.ReadErrorAsync() });

            return Json(new { Count = await CartItemsCount() });
        }

        [HttpDelete("DeleteItem/{itemId:int}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var client = _httpClientFactory.CreateClient("ePizzaAPI");
            var response = await client.DeleteAsync($"Cart/delete-item?cartId={CartId}&itemId={itemId}");

            if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                return StatusCode((int)response.StatusCode, new { Message = await response.ReadErrorAsync() });

            return Json(new { Count = await CartItemsCount() });
        }

        [Authorize]
        [HttpGet("Checkout")]
        public IActionResult Checkout() => View();

        [Authorize]
        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout(AddressViewModel address)
        {
            if (!ModelState.IsValid) return View(address);

            var client = _httpClientFactory.CreateClient("ePizzaAPI");

            var cart = await client.GetFromJsonAsync<GetCartResponseModel>($"Cart/get-cart-details?cartId={CartId}");
            if (cart is null || cart.Items.Count == 0) return RedirectToAction("Index");

            // Guest cart ko login user se jodo (API body ka UserId ignore karti hai, token se leti hai)
            var link = await client.PutAsJsonAsync("Cart/update-cart-user",
                new { CartId, UserId = CurrentUser!.UserId });

            if (link.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("LogOut", "Login");     // token expire ho gaya
            if (!link.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, await link.ReadErrorAsync());
                return View(address);
            }

            TempData.Set("Address", address);                   // PRG pattern: POST, phir Redirect
            return RedirectToAction("Index", "Payment");
        }

        private async Task<int> CartItemsCount()
        {
            var client = _httpClientFactory.CreateClient("ePizzaAPI");
            return await client.GetFromJsonAsync<int>($"Cart/get-item-count?guid={CartId}");
        }
    }
}
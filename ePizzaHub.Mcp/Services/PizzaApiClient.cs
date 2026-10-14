using System.Net;
using System.Net.Http.Json;
using ePizzaHub.Mcp.Models;
using ModelContextProtocol;

namespace ePizzaHub.Mcp.Services
{
    public class PizzaApiClient
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _accessor;
        public PizzaApiClient(HttpClient http, IHttpContextAccessor accessor)
        {
            _http = http;
            _accessor = accessor;
        }

        // Cart id host se header me aata hai, LLM ke argument se kabhi nahi
        private Guid CartId =>
            Guid.TryParse(_accessor.HttpContext?.Request.Headers["X-Cart-Id"].ToString(), out var id) ? id
            : throw new McpException("No cart is attached to this chat session.");

        public async Task<List<MenuItem>> GetMenuAsync()
            => await Read<List<MenuItem>>(await _http.GetAsync("Item")) ?? new();

        public async Task<CartView> GetCartAsync()
            => await Read<CartView>(await _http.GetAsync($"Cart/get-cart-details?cartId={CartId}"))
               ?? new CartView(0, 0, 0, new());

        public async Task AddToCartAsync(int itemId, int quantity)
           => await Read<bool>(await _http.PostAsJsonAsync("Cart/add-item-to-cart",
               new { ItemId = itemId, Quantity = quantity, CartId }));

        public async Task RemoveFromCartAsync(int itemId)
            => await Read<bool>(await _http.DeleteAsync($"Cart/delete-item?cartId={CartId}&itemId={itemId}"));

        public async Task<List<OrderView>> GetOrdersAsync()
            => await Read<List<OrderView>>(await _http.GetAsync("Order")) ?? new();

        // API ke HTTP errors ko aise message me badlo jo model samajh sake
        private static async Task<T?> Read<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<T>();

            throw new Exception(response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "The user is not logged in. Ask them to log in first.",
                HttpStatusCode.NotFound => "That item or cart was not found.",
                HttpStatusCode.BadRequest => "The request was invalid (check item id and quantity).",
                _ => "The pizza service has a problem. Try again later."
            });
        }
    }
}

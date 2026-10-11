using ePizzaHub.Core.Contracts;
using ePizzaHub.Core.Settings;
using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Models.ApiModels.Response;
using ePizzaHub.Repositories.Contracts;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Concrete
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;
        private readonly PricingSettings _pricing;

        public CartService(ICartRepository repo, IOptions<PricingSettings> pricing)
        {
            _repo = repo;
            _pricing = pricing.Value;
        }

        public async Task<bool> AddItemAsync(AddToCartRequest request)
        {
            await _repo.AddItemAsync(request.cartId, request.UserId, request.ItemId, request.Quantity);
            return true;
        }

        public async Task<CartResponseModel> GetCartDetailsAsync(Guid cartId)
        {
            var cart = await _repo.GetDetailsAsync(cartId);
            if (cart is null) return new CartResponseModel();

            cart.Tax = Math.Round(cart.Total * _pricing.TaxRate, 2, MidpointRounding.AwayFromZero);
            cart.GrandTotal = cart.Total + cart.Tax;

            return cart;
        }

        public Task<int> GetItemCountAsync(Guid cartId)
        {
            return _repo.GetItemCountAsync(cartId);
        }

        public async Task<bool> RemoveItemAsync(Guid cartId, int itemId)
        {
            return await _repo.RemoveItemAsync(cartId, itemId) > 0;
        }

        public async Task UpdateCartUserAsync(Guid cartId, int userId)
        {
            await _repo.UpdateUserAsync(cartId, userId);
        }

        public async Task<bool> UpdateItemQuantityAsync(Guid cartId, int itemId, int quantity)
        {
            await _repo.UpdateItemQuantityAsync(cartId, itemId, quantity);
            return true;
        }
    }
}

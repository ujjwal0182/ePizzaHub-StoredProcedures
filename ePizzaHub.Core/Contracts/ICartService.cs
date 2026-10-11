using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Models.ApiModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Contracts
{
    public interface ICartService
    {
        Task<bool> AddItemAsync(AddToCartRequest request);
        Task<CartResponseModel> GetCartDetailsAsync(Guid cartId);
        Task<int> GetItemCountAsync(Guid cartId);
        Task<bool> UpdateItemQuantityAsync(Guid cartId, int itemId, int quantity);
        Task<bool> RemoveItemAsync(Guid cartId, int itemId);
        Task UpdateCartUserAsync(Guid cartId, int userId);
    }
}

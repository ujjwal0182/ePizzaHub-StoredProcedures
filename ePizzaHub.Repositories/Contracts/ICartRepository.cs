using ePizzaHub.Models.ApiModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Contracts
{
    public interface ICartRepository
    {
        Task AddItemAsync(Guid cartId, int userId, int itemId, int quantity);
        Task<CartResponseModel?> GetDetailsAsync(Guid cartId);
        Task<int> GetItemCountAsync(Guid cartId);
        Task UpdateItemQuantityAsync(Guid cartId, int itemId, int quantity);
        Task<int> RemoveItemAsync(Guid cartId, int itemId);
        Task UpdateUserAsync(Guid cartId, int userId);
    }
}

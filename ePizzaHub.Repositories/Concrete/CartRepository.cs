using Dapper;
using ePizzaHub.Models.ApiModels.Response;
using ePizzaHub.Repositories.Contracts;
using ePizzaHub.Repositories.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Concrete
{
    public class CartRepository : ICartRepository
    {
        private readonly IDbConnectionFactory _factory;
        public CartRepository(IDbConnectionFactory factory) => _factory = factory;

        public async Task AddItemAsync(Guid cartId, int userId, int itemId, int quantity)
        {
            using var conn = _factory.Create();
            await conn.ExecuteAsync("dbo.usp_Cart_AddItem",
                new {
                    CartId  = cartId,
                    UserId  = userId,
                    ItemId  = itemId,
                    Quantity  = quantity
                },
                commandType:CommandType.StoredProcedure);
        }

        public async Task<CartResponseModel?> GetDetailsAsync(Guid cartId)
        {
            using var conn = _factory.Create();
            using var multi = await conn.QueryMultipleAsync("dbo.usp_Cart_GetDetails",
                new{
                    CartId = cartId
                },
                commandType: CommandType.StoredProcedure);

            var cart = await multi.ReadSingleOrDefaultAsync<CartResponseModel>();
            if (cart is null) return null;

            cart.Items = (await multi.ReadAsync<CartItemResponseModel>()).ToList();
            return cart;
        }

        public async Task<int> GetItemCountAsync(Guid cartId)
        {
            using var conn = _factory.Create();
            return await conn.ExecuteScalarAsync<int>("dbo.usp_Cart_GetItemCount", 
                new { 
                    CartId = cartId 
                }, 
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> RemoveItemAsync(Guid cartId, int itemId)
        {
            using var conn = _factory.Create();
            return await conn.ExecuteScalarAsync<int>("dbo.usp_Cart_RemoveItem",
                new
                {
                    CartId = cartId,
                    ItemId = itemId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateItemQuantityAsync(Guid cartId, int itemId, int quantity)
        {
            using var conn = _factory.Create();
            await conn.ExecuteAsync("dbo.usp_Cart_UpdateItemQuantity",
                new
                {
                    CartId = cartId,
                    ItemId = itemId,
                    Quantity = quantity
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateUserAsync(Guid cartId, int userId)
        {
            using var conn = _factory.Create();
            await conn.ExecuteAsync("dbo.usp_Cart_UpdateUser",
                new
                {
                    CartId = cartId,
                    UserId = userId
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}

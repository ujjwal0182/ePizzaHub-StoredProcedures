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
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnectionFactory _factory;
        public OrderRepository(IDbConnectionFactory factory) => _factory = factory;
        public async Task<List<OrderResponseModel>> GetByUserAsync(int userId)
        {
            using var conn = _factory.Create();
            using var multi = await conn.QueryMultipleAsync("dbo.usp_Order_GetByUser",
                new { UserId = userId }, commandType: CommandType.StoredProcedure);

            var orders = (await multi.ReadAsync<OrderResponseModel>()).ToList();
            var lines = (await multi.ReadAsync<OrderItemResponseModel>()).ToLookup(x => x.OrderId);

            foreach (var order in orders)
                order.Items = lines[order.Id].ToList();   // memory me join, extra DB call nahi

            return orders;
        }
    }
}

using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Response;
using ePizzaHub.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        public OrderService(IOrderRepository repo) => _repo = repo;
        public Task<List<OrderResponseModel>> GetOrdersAsync(int userId)
        {
            return _repo.GetByUserAsync(userId);
        }
    }
}

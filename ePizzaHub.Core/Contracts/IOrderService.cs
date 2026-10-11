using ePizzaHub.Models.ApiModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Contracts
{
    public interface IOrderService
    {
        Task<List<OrderResponseModel>> GetOrdersAsync(int userId);
    }
}

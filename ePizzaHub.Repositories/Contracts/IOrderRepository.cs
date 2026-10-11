using ePizzaHub.Models.ApiModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Contracts
{
    public interface IOrderRepository
    {
        Task<List<OrderResponseModel>> GetByUserAsync(int userId);
    }
}

using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Models.ApiModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Contracts
{
    public interface IPaymentService
    {
        Task<CreateOrderResponse> CreateOrderAsync(Guid cartId, int userId);
        Task<string> MakePaymentAsync(MakePaymentRequest request, int userId, string email);
    }
}

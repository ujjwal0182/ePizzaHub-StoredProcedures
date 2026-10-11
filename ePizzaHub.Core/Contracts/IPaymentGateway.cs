using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Contracts
{
    public record GatewayOrder(string OrderId, long AmountInPaise, string Currency);
    public record GatewayPayment(string PaymentId, string OrderId, string Status, long AmountInPaise, string Currency);

    public interface IPaymentGateway
    {
        Task<GatewayOrder> CreateOrderAsync(long amountInPaise, string currency, string receipt);
        Task<GatewayPayment> GetPaymentAsync(string paymentId);
    }
}

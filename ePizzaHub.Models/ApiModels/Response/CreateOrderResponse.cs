using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Models.ApiModels.Response
{
    public class CreateOrderResponse
    {
        public string OrderId { get; set; } = string.Empty;   // Razorpay order id
        public long Amount { get; set; }                      // paise
        public string Currency { get; set; } = "INR";
        public string KeyId { get; set; } = string.Empty;     // public key, client me jaana safe hai
    }
}

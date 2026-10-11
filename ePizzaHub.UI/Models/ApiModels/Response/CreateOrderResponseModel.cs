namespace ePizzaHub.UI.Models.ApiModels.Response
{
    public class CreateOrderResponseModel
    {
        public string OrderId { get; set; } = string.Empty;
        public long Amount { get; set; }                    // paise
        public string Currency { get; set; } = "INR";
        public string KeyId { get; set; } = string.Empty;
    }

    public class MakePaymentResponseModel
    {
        public string OrderId { get; set; } = string.Empty;
    }
}
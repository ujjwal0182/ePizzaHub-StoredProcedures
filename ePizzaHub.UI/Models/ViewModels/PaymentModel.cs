namespace ePizzaHub.UI.Models.ViewModels
{
    public class PaymentModel
    {
        public string Name { get; set; } = "ePizzaHub";
        public string RazorPayKey { get; set; } = string.Empty;
        public long AmountInPaise { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Currency { get; set; } = "INR";
        public string OrderId { get; set; } = string.Empty;
        public GetCartResponseModel Cart { get; set; } = default!;
    }
}
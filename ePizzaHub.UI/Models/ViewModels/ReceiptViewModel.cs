namespace ePizzaHub.UI.Models.ViewModels
{
    public class ReceiptViewModel
    {
        public string TransactionId { get; set; }
        public decimal Tax { get; set; }
        public string Currency { get; set; }
        public decimal Total { get; set; }
        public decimal GrandTotal { get; set; }        
    }
}

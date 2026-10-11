namespace ePizzaHub.UI.Models.ViewModels
{
    // This model will be used to get the cart details from the API and display it in the cart page.
    public class GetCartResponseModel
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public List<CartItemResponseModel> Items { get; set; } // List of items in the cart
    }
    public class CartItemResponseModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ItemName { get; set; }
        public string ImageUrl { get; set; }
        public decimal ItemTotal 
        { 
            get
            {
                return UnitPrice * Quantity;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Models.ApiModels.Response
{
    public class CartResponseModel
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public List<CartItemResponseModel> Items { get; set; } = new(); // List of items in the cart
    }
    public class CartItemResponseModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ItemName { get; set; }
        public string ImageUrl { get; set; }
    }
}

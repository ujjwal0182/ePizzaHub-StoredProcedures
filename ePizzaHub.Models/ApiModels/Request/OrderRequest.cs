using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Models.ApiModels.Request
{
    public class OrderRequest
    {
        public OrderRequest()
        {
            OrderItems = new List<OrderItems>();
        }
        public string Id { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Locality { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
        public int UserId { get; set; }

        /// <summary>
        /// Represents a collection of items in a single order.
        /// One OrderRequest can contain multiple OrderItems (One-to-Many relationship).
        /// Initialized in constructor to avoid null reference issues.
        /// </summary>
        public ICollection<OrderItems> OrderItems { get; set; }

    }
    public class OrderItems
    {
        public int ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}

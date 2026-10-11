using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Models.ApiModels.Request
{
    public class AddToCartRequest
    {
        public int UserId { get; set; }
        public Guid cartId { get; set; }
        [Range(1, int.MaxValue)]
        public int ItemId { get; set; }
        [Range(1, 50)]
        public int Quantity { get; set; }
    }
}

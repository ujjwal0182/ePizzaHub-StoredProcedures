using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Models.ApiModels.Request
{
    public class CreateOrderRequest
    {
        public Guid CartId { get; set; }
    }
}

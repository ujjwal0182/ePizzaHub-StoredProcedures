using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Models.ApiModels.Request
{
    public class UpdateCartUserRequest
    {
        public Guid CartId { get; set; }
        public int UserId { get; set; }
    }
}

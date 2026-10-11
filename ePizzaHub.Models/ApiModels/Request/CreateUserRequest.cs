using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Models.ApiModels.Request
{
    public class CreateUserRequest
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [Required, EmailAddress, StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required, Phone,StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}

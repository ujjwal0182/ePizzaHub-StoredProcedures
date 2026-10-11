using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Entities
{
    public class UserRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;   // BCrypt hash
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}

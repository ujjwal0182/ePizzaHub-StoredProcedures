using ePizzaHub.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<UserRecord?> GetByEmailAsync(string email);
        Task<int> CreateAsync(string name, string email, string passwordHash, string phoneNumber);
    }
}

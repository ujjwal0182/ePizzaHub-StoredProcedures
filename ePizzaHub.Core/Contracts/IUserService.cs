using ePizzaHub.Models.ApiModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Contracts
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(CreateUserRequest request);
    }
}

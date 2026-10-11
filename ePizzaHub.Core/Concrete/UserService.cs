using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

        public async Task<bool> CreateUserAsync(CreateUserRequest request)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);   // hashing C# me, SQL me nahi
            var newId = await _repo.CreateAsync(request.Name.Trim(), request.Email.Trim().ToLowerInvariant(), hash, request.PhoneNumber);
            return newId > 0;
        }
    }
}

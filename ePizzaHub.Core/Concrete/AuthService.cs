using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Response;
using ePizzaHub.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        public AuthService(IUserRepository repo) => _repo = repo;

        public async Task<ValidateUserResponse> ValidateUserAsync(string email, string password)
        {
            var user = await _repo.GetByEmailAsync(email.Trim().ToLowerInvariant());

            if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new UnauthorizedAccessException("Invalid email or password.");   // same message dono case me

            return new ValidateUserResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Roles = user.Roles
            };
        }
    }
}

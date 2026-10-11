using ePizzaHub.Core.Concrete;
using ePizzaHub.Repositories.Contracts;
using ePizzaHub.Repositories.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _repo = new();
        private AuthService Sut() => new (_repo.Object);

        private static UserRecord User(string password) => new()
        {
            Id = 1,
            Name = "Ujjwal",
            Email = "a@x.com",
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Roles = new List<string> { "User" }
        };

        [Fact]
        public async Task ValidateUser_WrongPassword_Throws()
        {
            _repo.Setup(r => r.GetByEmailAsync("a@x.com")).ReturnsAsync(User("Right@123"));

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => Sut().ValidateUserAsync("a@x.com", "wrong"));
        }

        [Fact]
        public async Task ValidateUser_UnknownEmail_ThrowsSameException()
        {
            _repo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((UserRecord?)null);

            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => Sut().ValidateUserAsync("nobody@x.com", "whatever"));
            Assert.Equal("Invalid email or password.", ex.Message);
        }

        [Fact]
        public async Task ValidateUser_CorrectPassword_ReturnsUserWithRoles()
        {
            _repo.Setup(r => r.GetByEmailAsync("a@x.com")).ReturnsAsync(User("Right@123"));

            var result = await Sut().ValidateUserAsync("a@x.com", "Right@123");

            Assert.Equal(1, result.UserId);
            Assert.Contains("User", result.Roles);
        }

        [Fact]
        public async Task ValidateUser_NormalizesEmail()
        {
            _repo.Setup(r => r.GetByEmailAsync("a@x.com")).ReturnsAsync(User("Right@123"));

            await Sut().ValidateUserAsync("  A@X.com ", "Right@123");

            _repo.Verify(r => r.GetByEmailAsync("a@x.com"), Times.Once);
        }
    }
}

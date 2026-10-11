using ePizzaHub.Core.Concrete;
using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Repositories.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task CreateUser_StoresBcryptHash_NotPlainPassword_AndLowercasesEmail()
        {
            string? savedEmail = null, savedHash = null;
            var repo = new Mock<IUserRepository>();
            repo.Setup(r => r.CreateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string>((n, e, h, p) => { savedEmail = e; savedHash = h; })
                .ReturnsAsync(5);

            var ok = await new UserService(repo.Object).CreateUserAsync(new CreateUserRequest
            {
                Name = "Test",
                Email = " Test@X.com ",
                Password = "Test@1234",
                PhoneNumber = "9999999999"
            });

            Assert.True(ok);
            Assert.Equal("test@x.com", savedEmail);
            Assert.NotEqual("Test@1234", savedHash);
            Assert.True(BCrypt.Net.BCrypt.Verify("Test@1234", savedHash));
        }
    }
}

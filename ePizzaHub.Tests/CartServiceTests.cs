using ePizzaHub.Core.Concrete;
using ePizzaHub.Core.Settings;
using ePizzaHub.Models.ApiModels.Response;
using ePizzaHub.Repositories.Contracts;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Tests
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _repo = new();
        private CartService Sut() =>
            new(_repo.Object, Options.Create(new PricingSettings { TaxRate = 0.05m }));
        
        public async Task GetCartDetails_CalculatesTaxAndGrandTotal()
        {
            var id = Guid.NewGuid();
            _repo.Setup(r => r.GetDetailsAsync(id)).ReturnsAsync(new CartResponseModel { Id = id, Total = 997.00m });

            var cart = await Sut().GetCartDetailsAsync(id);

            Assert.Equal(49.85m, cart.Tax);
            Assert.Equal(1046.85m, cart.GrandTotal);
        }

        [Fact]
        public async Task GetCartDetails_CartMissing_ReturnsEmptyModel()
        {
            _repo.Setup(r => r.GetDetailsAsync(It.IsAny<Guid>())).ReturnsAsync((CartResponseModel?)null);

            var cart = await Sut().GetCartDetailsAsync(Guid.NewGuid());

            Assert.Empty(cart.Items);
            Assert.Equal(0m, cart.GrandTotal);
        }

        [Fact]
        public async Task RemoveItem_NothingDeleted_ReturnsFalse()
        {
            _repo.Setup(r => r.RemoveItemAsync(It.IsAny<Guid>(), It.IsAny<int>())).ReturnsAsync(0);

            Assert.False(await Sut().RemoveItemAsync(Guid.NewGuid(), 1));
        }

        [Fact]
        public async Task GetCartDetails_TaxRoundsHalfUp_LikeSql()
        {
            var id = Guid.NewGuid();
            _repo.Setup(r => r.GetDetailsAsync(id)).ReturnsAsync(new CartResponseModel { Id = id, Total = 100.10m });

            var cart = await Sut().GetCartDetailsAsync(id);

            Assert.Equal(5.01m, cart.Tax);   // 100.10 * 0.05 = 5.005. Banker's rounding 5.00 deta
        }
    }
}

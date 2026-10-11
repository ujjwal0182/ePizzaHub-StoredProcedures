using ePizzaHub.Core.Concrete;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Core.Exceptions;
using ePizzaHub.Core.Helpers;
using ePizzaHub.Core.Settings;
using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Models.ApiModels.Response;
using ePizzaHub.Repositories.Contracts;
using ePizzaHub.Repositories.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace ePizzaHub.Tests
{
    public class PaymentServiceTests
    {
        private const string Secret = "test_secret";
        private readonly Mock<IPaymentRepository> _repo = new();
        private readonly Mock<ICartService> _cart = new();
        private readonly Mock<IPaymentGateway> _gateway = new();

        private PaymentService Sut() => new(
            _repo.Object, _cart.Object, _gateway.Object,
            Options.Create(new PricingSettings { TaxRate = 0.05m }),
            Options.Create(new RazorpaySettings { KeyId = "rzp_test_x", KeySecret = Secret }),
            NullLogger<PaymentService>.Instance);

        private static MakePaymentRequest Request(string signature) => new()
        {
            CartId = Guid.NewGuid(),
            RazorpayOrderId = "order_1",
            RazorpayPaymentId = "pay_1",
            RazorpaySignature = signature,
            Street = " MG Road ",
            City = "Mohali",
            Locality = "Phase 7",
            ZipCode = "160055",
            PhoneNumber = "9999999999"
        };

        private static string ValidSignature => PaymentSignature.Compute(Secret, "order_1", "pay_1");

        [Fact]
        public async Task MakePayment_InvalidSignature_Throws_AndNothingIsCalled()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(
                () => Sut().MakePaymentAsync(Request("tampered"), 7, "u@x.com"));

            _gateway.Verify(g => g.GetPaymentAsync(It.IsAny<string>()), Times.Never);
            _repo.Verify(r => r.MakePaymentAsync(It.IsAny<MakePaymentCommand>()), Times.Never);
        }

        [Fact]
        public async Task MakePayment_PaymentFailedAtGateway_Throws()
        {
            _gateway.Setup(g => g.GetPaymentAsync("pay_1"))
                .ReturnsAsync(new GatewayPayment("pay_1", "order_1", "failed", 104685, "INR"));

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => Sut().MakePaymentAsync(Request(ValidSignature), 7, "u@x.com"));

            _repo.Verify(r => r.MakePaymentAsync(It.IsAny<MakePaymentCommand>()), Times.Never);
        }

        [Fact]
        public async Task MakePayment_Valid_UsesAmountAndStatusFromGateway_NotFromClient()
        {
            _gateway.Setup(g => g.GetPaymentAsync("pay_1"))
                .ReturnsAsync(new GatewayPayment("pay_1", "order_1", "captured", 104685, "INR"));
            _repo.Setup(r => r.MakePaymentAsync(It.IsAny<MakePaymentCommand>())).ReturnsAsync("order_1");

            var orderId = await Sut().MakePaymentAsync(Request(ValidSignature), 7, "u@x.com");

            Assert.Equal("order_1", orderId);
            _repo.Verify(r => r.MakePaymentAsync(It.Is<MakePaymentCommand>(c =>
                c.UserId == 7 && c.Email == "u@x.com" && c.TaxRate == 0.05m &&
                c.PaidAmount == 1046.85m && c.Status == "captured" && c.Street == "MG Road")), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_CartBelongsToSomeoneElse_Throws()
        {
            var id = Guid.NewGuid();
            _cart.Setup(c => c.GetCartDetailsAsync(id)).ReturnsAsync(new CartResponseModel
            {
                Id = id,
                UserId = 99,
                GrandTotal = 100m,
                Items = new() { new CartItemResponseModel { ItemId = 1, Quantity = 1, UnitPrice = 100m } }
            });

            await Assert.ThrowsAsync<BusinessRuleException>(() => Sut().CreateOrderAsync(id, 7));
        }

        [Fact]
        public async Task CreateOrder_Valid_SendsAmountInPaise()
        {
            var id = Guid.NewGuid();
            _cart.Setup(c => c.GetCartDetailsAsync(id)).ReturnsAsync(new CartResponseModel
            {
                Id = id,
                UserId = 7,
                GrandTotal = 1046.85m,
                Items = new() { new CartItemResponseModel { ItemId = 1, Quantity = 1, UnitPrice = 997m } }
            });
            _gateway.Setup(g => g.CreateOrderAsync(104685, "INR", id.ToString("N")))
                .ReturnsAsync(new GatewayOrder("order_9", 104685, "INR"));

            var result = await Sut().CreateOrderAsync(id, 7);

            Assert.Equal("order_9", result.OrderId);
            Assert.Equal(104685, result.Amount);
            Assert.Equal("rzp_test_x", result.KeyId);
        }
    }
}
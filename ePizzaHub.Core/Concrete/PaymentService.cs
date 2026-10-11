using ePizzaHub.Core.Contracts;
using ePizzaHub.Core.Exceptions;
using ePizzaHub.Core.Helpers;
using ePizzaHub.Core.Settings;
using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Models.ApiModels.Response;
using ePizzaHub.Repositories.Contracts;
using ePizzaHub.Repositories.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ePizzaHub.Core.Concrete
{
    public class PaymentService : IPaymentService
    {
        // Razorpay me auto-capture off ho to payment "authorized" rehta hai, dono accept
        private static readonly string[] SuccessStatuses = { "captured", "authorized" };

        private readonly IPaymentRepository _repo;
        private readonly ICartService _cartService;
        private readonly IPaymentGateway _gateway;
        private readonly PricingSettings _pricing;
        private readonly RazorpaySettings _razorpay;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IPaymentRepository repo, ICartService cartService, IPaymentGateway gateway,
            IOptions<PricingSettings> pricing, IOptions<RazorpaySettings> razorpay, ILogger<PaymentService> logger)
        {
            _repo = repo;
            _cartService = cartService;
            _gateway = gateway;
            _pricing = pricing.Value;
            _razorpay = razorpay.Value;
            _logger = logger;
        }

        public async Task<CreateOrderResponse> CreateOrderAsync(Guid cartId, int userId)
        {
            var cart = await _cartService.GetCartDetailsAsync(cartId);
            if (cart.Items.Count == 0 || cart.UserId != userId)
                throw new BusinessRuleException("Cart is empty or does not belong to this user.");

            // Razorpay amount paise me leta hai
            var paise = (long)Math.Round(cart.GrandTotal * 100m, MidpointRounding.AwayFromZero);
            var order = await _gateway.CreateOrderAsync(paise, "INR", Receipt(cartId));

            return new CreateOrderResponse
            {
                OrderId = order.OrderId,
                Amount = order.AmountInPaise,
                Currency = order.Currency,
                KeyId = _razorpay.KeyId
            };
        }

        public async Task<string> MakePaymentAsync(MakePaymentRequest r, int userId, string email)
        {
            // 1) Signature: ye request waqai Razorpay checkout se aayi hai?
            if (!PaymentSignature.IsValid(_razorpay.KeySecret, r.RazorpayOrderId, r.RazorpayPaymentId, r.RazorpaySignature))
                throw new BusinessRuleException("Invalid payment signature.");

            // 2) Asli amount aur status Razorpay se, client se nahi
            var payment = await _gateway.GetPaymentAsync(r.RazorpayPaymentId);

            if (payment.OrderId != r.RazorpayOrderId)
                throw new BusinessRuleException("Payment does not belong to this order.");

            if (!SuccessStatuses.Contains(payment.Status))
                throw new BusinessRuleException($"Payment is not successful (status: {payment.Status}).");

            var command = new MakePaymentCommand(
                PaymentId: payment.PaymentId,
                TransactionId: Receipt(r.CartId),
                OrderId: payment.OrderId,
                CartId: r.CartId,
                UserId: userId,
                Email: email,
                Currency: payment.Currency,
                Status: payment.Status,
                PaidAmount: payment.AmountInPaise / 100m,   // paise to rupees
                TaxRate: _pricing.TaxRate,
                Street: r.Street.Trim(), City: r.City.Trim(), Locality: r.Locality.Trim(),
                ZipCode: r.ZipCode.Trim(), PhoneNumber: r.PhoneNumber.Trim());

            try
            {
                // 3) SP cart ke server-side total se amount match karti hai, phir order banati hai
                return await _repo.MakePaymentAsync(command);
            }
            catch (Exception ex)
            {
                // Paisa kat chuka hai par order save nahi hua. Isko chhupana nahi, manual refund/reconcile
                _logger.LogCritical(ex,
                    "Payment {PaymentId} is {Status} at gateway but order was NOT saved for cart {CartId}. Reconcile or refund.",
                    payment.PaymentId, payment.Status, r.CartId);
                throw;
            }
        }

        // Razorpay receipt max 40 chars. Cart id ka "N" format 32 chars ka hai
        private static string Receipt(Guid cartId) => cartId.ToString("N");
    }
}
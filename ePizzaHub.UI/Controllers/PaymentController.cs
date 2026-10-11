using System.Net;
using ePizzaHub.UI.Helpers;
using ePizzaHub.UI.Models.ApiModels.Request;
using ePizzaHub.UI.Models.ApiModels.Response;
using ePizzaHub.UI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Controllers
{
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public PaymentController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<IActionResult> Index()
        {
            var address = TempData.Peek<AddressViewModel>("Address");
            if (address is null || !Guid.TryParse(Request.Cookies["CartId"], out var cartId))
                return RedirectToAction("Index", "Cart");

            var client = _httpClientFactory.CreateClient("ePizzaAPI");

            var cart = await client.GetFromJsonAsync<GetCartResponseModel>($"Cart/get-cart-details?cartId={cartId}");
            if (cart is null || cart.Items.Count == 0) return RedirectToAction("Index", "Cart");

            // Razorpay order API banati hai (amount cart se, paise me). UI amount ko chhuti bhi nahi.
            var response = await client.PostAsJsonAsync("Payment/create-order", new { CartId = cartId });
            if (response.StatusCode == HttpStatusCode.Unauthorized) return RedirectToAction("LogOut", "Login");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Message = await response.ReadErrorAsync();
                return View("Status");
            }

            var order = (await response.Content.ReadFromJsonAsync<CreateOrderResponseModel>())!;
            TempData.Set("CartDetail", cart);            // receipt me totals dikhane ke liye

            return View(new PaymentModel
            {
                RazorPayKey = order.KeyId,
                AmountInPaise = order.Amount,
                Currency = order.Currency,
                OrderId = order.OrderId,
                Description = "Payment for Order",
                Cart = cart
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Status(IFormCollection form)
        {
            var paymentId = form["rzp_paymentid"].ToString();
            var orderId = form["rzp_orderid"].ToString();
            var signature = form["rzp_signature"].ToString();

            var address = TempData.Peek<AddressViewModel>("Address");
            var cart = TempData.Peek<GetCartResponseModel>("CartDetail");

            if (cart is null || address is null || string.IsNullOrEmpty(paymentId)
                || string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(signature))
            {
                ViewBag.Message = "Payment session expired. Please check your orders before trying again.";
                return View();
            }

            var request = new MakePaymentRequestModel
            {
                CartId = cart.Id,
                RazorpayOrderId = orderId,
                RazorpayPaymentId = paymentId,
                RazorpaySignature = signature,
                Street = address.Street,
                City = address.City,
                Locality = address.Locality,
                ZipCode = address.ZipCode,
                PhoneNumber = address.PhoneNumber
            };

            var client = _httpClientFactory.CreateClient("ePizzaAPI");
            var response = await client.PostAsJsonAsync("Payment", request);

            if (!response.IsSuccessStatusCode)
            {
                // Paisa kat chuka ho sakta hai, isliye payment id user ko dikhate hain
                ViewBag.Message = $"We could not confirm your order: {await response.ReadErrorAsync()} " +
                                  $"If money was deducted, contact support with payment id {paymentId}.";
                return View();
            }

            var result = (await response.Content.ReadFromJsonAsync<MakePaymentResponseModel>())!;

            Response.Cookies.Delete("CartId");           // cart ab order ban chuka hai
            TempData.Remove("CartDetail");
            TempData.Remove("Address");
            TempData.Set("Receipt", new ReceiptViewModel
            {
                TransactionId = result.OrderId,
                Currency = "INR",
                Total = cart.Total,
                Tax = cart.Tax,
                GrandTotal = cart.GrandTotal
            });
            return RedirectToAction("Receipt");
        }

        public IActionResult Receipt()
        {
            var model = TempData.Peek<ReceiptViewModel>("Receipt");
            if (model is null) return RedirectToAction("Index", "Dashboard");
            TempData.Remove("Receipt");
            return View(model);
        }
    }
}
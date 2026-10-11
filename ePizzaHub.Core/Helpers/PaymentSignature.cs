using System.Security.Cryptography;
using System.Text;

namespace ePizzaHub.Core.Helpers
{
    public static class PaymentSignature
    {
        public static string Compute(string secret, string orderId, string paymentId)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes($"{orderId}|{paymentId}"));
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        public static bool IsValid(string secret, string orderId, string paymentId, string? signature)
        {
            if (string.IsNullOrEmpty(signature)) return false;
            var expected = Encoding.UTF8.GetBytes(Compute(secret, orderId, paymentId));
            // constant-time compare: timing attack se bachne ke liye ==/Equals nahi
            return CryptographicOperations.FixedTimeEquals(expected, Encoding.UTF8.GetBytes(signature));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Entities
{
    public record MakePaymentCommand(
        string PaymentId, string TransactionId, string OrderId,
        Guid CartId, int UserId, string Email,
        string Currency, string Status, decimal PaidAmount, decimal TaxRate,
        string Street, string City, string Locality, string ZipCode, string PhoneNumber);
}

using Dapper;
using ePizzaHub.Repositories.Contracts;
using ePizzaHub.Repositories.Entities;
using ePizzaHub.Repositories.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Concrete
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDbConnectionFactory _factory;
        public PaymentRepository(IDbConnectionFactory factory) => _factory = factory;
        public async Task<string> MakePaymentAsync(MakePaymentCommand c)
        {
            using var conn = _factory.Create();
            var orderId = await conn.ExecuteScalarAsync<string>("dbo.usp_Payment_Make", 
                new
                {
                    c.PaymentId,
                    c.TransactionId,
                    c.OrderId,
                    c.CartId,
                    c.UserId,
                    c.Email,
                    c.Currency,
                    c.Status,
                    c.PaidAmount,
                    c.TaxRate,
                    c.Street,
                    c.City,
                    c.Locality,
                    c.ZipCode,
                    c.PhoneNumber
                }, 
                commandType: CommandType.StoredProcedure);
            return orderId ?? string.Empty;
        }
    }
}

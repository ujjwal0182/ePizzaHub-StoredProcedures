using ePizzaHub.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Contracts
{
    public interface IPaymentRepository
    {
        Task<string> MakePaymentAsync(MakePaymentCommand command);
    }
}

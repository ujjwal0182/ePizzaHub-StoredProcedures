using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Contracts
{
    public interface IAssistantService
    {
        Task<string> ChatAsync(string message, string cartId, string bearer, CancellationToken ct);
    }
}

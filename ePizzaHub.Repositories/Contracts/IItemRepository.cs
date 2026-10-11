using ePizzaHub.Models.ApiModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Contracts
{
    public interface IItemRepository
    {
        Task<IEnumerable<GetItemResponse>> GetAllAsync();
    }
}

using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Response;
using ePizzaHub.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Concrete
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repo;
        public ItemService(IItemRepository repo) => _repo = repo;

        public Task<IEnumerable<GetItemResponse>> GetItemsAsync() => _repo.GetAllAsync();
    }
}

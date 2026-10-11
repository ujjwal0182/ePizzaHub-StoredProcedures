using Dapper;
using ePizzaHub.Models.ApiModels.Response;
using ePizzaHub.Repositories.Contracts;
using ePizzaHub.Repositories.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Concrete
{
    public class ItemRepository : IItemRepository
    {
        private readonly IDbConnectionFactory _factory;
        public ItemRepository(IDbConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<GetItemResponse>> GetAllAsync()
        {
            using var conn = _factory.Create();
            return await conn.QueryAsync<GetItemResponse>(
                "dbo.usp_Item_GetAll",
                commandType: CommandType.StoredProcedure);
        }
    }
}

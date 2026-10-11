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
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _factory;
        public UserRepository(IDbConnectionFactory factory) => _factory = factory;

        public async Task<int> CreateAsync(string name, string email, string passwordHash, string phoneNumber)
        {
            using var conn = _factory.Create();
            var p = new DynamicParameters();
            p.Add("@Name", name);
            p.Add("@Email", email);
            p.Add("@PasswordHash", passwordHash);
            p.Add("@PhoneNumber", phoneNumber);
            p.Add("@NewUserId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("dbo.usp_User_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<int>("@NewUserId");
        }

        public async Task<UserRecord?> GetByEmailAsync(string email)
        {
            using var conn = _factory.Create();
            using var multi = await conn.QueryMultipleAsync(
                "dbo.usp_User_GetByEmail", new { Email = email }, commandType: CommandType.StoredProcedure
                );

            var user = await multi.ReadSingleOrDefaultAsync<UserRecord>();   // result set 1
            if (user is null) return null;

            user.Roles = (await multi.ReadAsync<string>()).ToList();          // result set 2
            return user;
        }
    }
}

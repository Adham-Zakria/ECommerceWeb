using Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer _connection) : ICacheRepository
    {
        private readonly IDatabase _database = _connection.GetDatabase();
        public async Task<string?> GetAsync(string cachekey)
        {
            var value = await _database.StringGetAsync(cachekey);
            return value.IsNullOrEmpty ? null : value.ToString();
        }

        public async Task SetAsync(string cachekey, string value, TimeSpan expiration)
        {
            await _database.StringSetAsync(cachekey, value, expiration);
        }
    }
}

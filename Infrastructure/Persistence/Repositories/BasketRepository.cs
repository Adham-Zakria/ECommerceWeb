using Domain.Contracts;
using Domain.Models.Baskets;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer _connection) : IBasketRepository
    {
        private readonly IDatabase _database=_connection.GetDatabase();

        public async Task<CustomerBasket?> GetAsync(string id)
        {
            var basket = await _database.StringGetAsync(id);
            if(basket.IsNullOrEmpty) return null;
            return JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }
        public async Task<CustomerBasket?> CreateOrUpdateAsync(CustomerBasket basket , TimeSpan? timeToLive = null)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            var isCreatedOrUpdated = await _database.StringSetAsync(basket.Id, jsonBasket , timeToLive ?? TimeSpan.FromDays(7)); // to store in database
            return  isCreatedOrUpdated ? await GetAsync(basket.Id) : null;
        }

        public async Task DeleteAsync(string id)
        {
            await _database.KeyDeleteAsync(id);
        }
    }
}

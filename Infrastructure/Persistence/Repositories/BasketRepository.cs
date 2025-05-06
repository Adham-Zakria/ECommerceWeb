using Domain.Contracts;
using Domain.Models.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        public Task<CustomerBasket> CreateOrUpdateAsync(CustomerBasket basket)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerBasket?> GetAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}

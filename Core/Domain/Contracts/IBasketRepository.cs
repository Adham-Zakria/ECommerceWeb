using Domain.Models.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetAsync(string id);
        Task<CustomerBasket?> CreateOrUpdateAsync(CustomerBasket basket , TimeSpan? timeToLive=null );
        Task DeleteAsync(string id);

    }
}

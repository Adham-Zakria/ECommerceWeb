using Shared.DataTransferObjects.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest, string email);
        Task<OrderResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<OrderResponse>> GetAllAsync(string email);
        Task<IEnumerable<DeliveryMethodResponse>> GetDeliveryMethodsAsync();
    }
}

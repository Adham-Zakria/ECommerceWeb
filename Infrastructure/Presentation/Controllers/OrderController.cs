using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DataTransferObjects.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")] // BaseUrl/api/order
    [ApiController]
    public class OrderController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder(OrderRequest request)
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            
            var res = await _serviceManager.OrderService.CreateOrderAsync(request, email);
            return Ok(res);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderResponse>> GetOrderById(Guid id)
        {
            return Ok(await _serviceManager.OrderService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _serviceManager.OrderService.GetAllAsync(email));
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodResponse>>> GetAllMethods()
        {
            return Ok(await _serviceManager.OrderService.GetDeliveryMethodsAsync());
        }

    }
}

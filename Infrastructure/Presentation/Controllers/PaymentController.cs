using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DataTransferObjects.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IServiceManager _serviceManager): ControllerBase
    {
        [HttpPost("{BasketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePayment(string BasketId)
        {
            var res = await _serviceManager.PaymentService.CreateOrUpdatePaymentIntent(BasketId);
            return Ok(res);
        }

        [HttpPost("WebHook")]
        public async Task<ActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            //logic
            await _serviceManager.PaymentService.UpdateOrderPaymentStatus(json, 
                                                    Request.Headers["Stripe-Signature"]);

            return new EmptyResult();
        }
    }
}

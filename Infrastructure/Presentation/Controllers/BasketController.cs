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
    [Route("api/[controller]")] // BaseUrl/api/Basket
    [ApiController]
    public class BasketController(IServiceManager _serviceManager) : ControllerBase
    {
        //1)Get the user basket
        [HttpGet]
        public async Task<ActionResult<BasketDto>> Get(string id) 
        {
            var basket = await _serviceManager.BasketService.GetAsync(id);
            return Ok(basket);
        }

        //2)Update the user basket
        //2.1) Create basket
        //2.2) add item to basket
        //2.3) remove item from basket
        //2.4) update basket item quantity +/-
        [HttpPost]
        public async Task<ActionResult<BasketDto>> Update(BasketDto basketDto)
        {
            var basket =  await _serviceManager.BasketService.UpdateAsync(basketDto);
            return Ok(basket);
        }

        //3)Delete the user basket => after checkout
        [HttpDelete]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            await _serviceManager.BasketService.DeleteAsync(id);
            return NoContent();
        }
        
    }
}

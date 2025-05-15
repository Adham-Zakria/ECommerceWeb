using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared;
using Shared.DataTransferObjects.Products;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")] // BaseUrl/api/Products
    [ApiController]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        // Get all products
        //[HttpGet] 
        //public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts([FromQuery]ProductQueryParameters productQueryParameters) 
        //{
        //    var products = await _serviceManager.ProductService.GetAllProductsAsync(productQueryParameters);
        //    return Ok(products);
        //}    // old one

        [HttpGet] //Get //BaseUrl/api/products
        public async Task<ActionResult<PaginatedResponse<ProductResponse>>> GetAllProducts([FromQuery]ProductQueryParameters productQueryParameters) 
        {
            var products = await _serviceManager.ProductService.GetAllProductsAsync(productQueryParameters);
            return Ok(products);
        }

        // Get product by id
        [HttpGet("{id}")] //Get //BaseUrl/api/products/{id}
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<ProductResponse>> GetProductById(int id)
        {
            var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(product);
        }

        // Get all brands
        [HttpGet("Brands")] //Get //BaseUrl/api/products/Brands
        public async Task<ActionResult<IEnumerable<BrandResponse>>> GetAllBrands()
        {
            var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }

        // Get all types
        [HttpGet("Types")] //Get //BaseUrl/api/products/Types
        public async Task<ActionResult<IEnumerable<TypeResponse>>> GetAllTypes()
        {
            var types = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);
        }
    }
}

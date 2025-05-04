using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DataTransferObjects.Products;
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
        [HttpGet] //Get //BaseUrl/api/products
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts() 
        {
            var products = await _serviceManager.ProductService.GetAllProductsAsync();
            return Ok(products);
        }

        // Get product by id
        [HttpGet("{id}")] //Get //BaseUrl/api/products/{id}
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

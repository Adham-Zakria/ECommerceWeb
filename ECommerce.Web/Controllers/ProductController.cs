using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("id")]
        public ActionResult<Product> GetById(int id) // BaseUrl/api/ControllerName/id
        {
            return new Product { Id = id };
        }

        [HttpGet]
        public ActionResult<Product> GetAll() // BaseUrl/api/ControllerName?id
        {
            return new Product { Id=10 };
        }

        [HttpPost]
        public ActionResult<Product> Add(Product product) 
        {
            return new Product { Id=product.Id,Name=product.Name};
        }

        [HttpPut]
        public ActionResult<Product> Update(Product product)
        {
            return new Product { Id = product.Id, Name = product.Name };
        }

        [HttpDelete]
        public ActionResult<Product> Delete(Product product) 
        {
            return new Product { Id=product.Id,Name=product.Name};
        }
    }

    public class Product 
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}

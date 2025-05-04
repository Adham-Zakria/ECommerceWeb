using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductWithTypeAndBrandSpecifications : BaseSpecifications<Product> 
    {
        // to get product by id
        public ProductWithTypeAndBrandSpecifications(int id):base(Prod=>Prod.Id==id) // filter by id
        {
            AddInclude(p=>p.ProductBrand);
            AddInclude(p=>p.ProductType);
        }

        // to get all products
        public ProductWithTypeAndBrandSpecifications() : base(null)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}

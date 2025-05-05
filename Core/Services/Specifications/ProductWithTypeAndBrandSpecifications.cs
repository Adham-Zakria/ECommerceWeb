using Domain.Models;
using Shared.DataTransferObjects.Products;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public ProductWithTypeAndBrandSpecifications(ProductQueryParameters productQueryParameters) : base(CreateCriteria(productQueryParameters))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            ApplySorting(productQueryParameters);
            ApplyPagination(productQueryParameters.PageSize, productQueryParameters.PageIndex);
        }

        private static Expression<Func<Product,bool>> CreateCriteria(ProductQueryParameters productQueryParameters)
        {
            return prod =>
                 (!productQueryParameters.BrandId.HasValue || prod.BrandId == productQueryParameters.BrandId.Value) &&
                 (!productQueryParameters.TypeId.HasValue || prod.TypeId == productQueryParameters.TypeId.Value) &&
                 (string.IsNullOrWhiteSpace(productQueryParameters.Search) ||
                  prod.Name.ToLower().Contains(productQueryParameters.Search.ToLower()));
        }

        private void ApplySorting(ProductQueryParameters productQueryParameters)
        {
            switch (productQueryParameters.ProductSortingOptions)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(prod => prod.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(prod => prod.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(prod => prod.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(prod => prod.Price);
                    break;
            }
        }
    }
}

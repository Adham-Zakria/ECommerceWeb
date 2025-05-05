using Shared;
using Shared.DataTransferObjects.Products;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
    public interface IProductService
    {
        // Get all products
        //Task<IEnumerable<ProductResponse>> GetAllProductsAsync(ProductQueryParameters productQueryParameters); //old one
        Task<PaginatedResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameters productQueryParameters);

        // Get product by id
        Task<ProductResponse> GetProductByIdAsync(int id);

        // Get all brands
        Task<IEnumerable<BrandResponse>> GetAllBrandsAsync();

        // Get all types
        Task<IEnumerable<TypeResponse>> GetAllTypesAsync();

    }
}

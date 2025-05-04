using Shared.DataTransferObjects.Products;
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
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();

        // Get product by id
        Task<ProductResponse> GetProductByIdAsync(int id);

        // Get all brands
        Task<IEnumerable<BrandResponse>> GetAllBrandsAsync();

        // Get all types
        Task<IEnumerable<TypeResponse>> GetAllTypesAsync();

    }
}

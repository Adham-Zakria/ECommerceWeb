using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Specifications;
using ServicesAbstraction;
using Shared;
using Shared.DataTransferObjects.Products;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService(IUnitOfWork _unitOfWork , IMapper _mapper) : IProductService
    {
        //public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync(ProductQueryParameters productQueryParameters)
        //{
        //    var specs = new ProductWithTypeAndBrandSpecifications(productQueryParameters); 

        //    var Repository =_unitOfWork.GetRepository<Product,int>();
        //    var products=await Repository.GetAllAsync(specs);
        //    //mapping from IEnumerable<Product> to IEnumerable<ProductResponse>
        //    var result =_mapper.Map<IEnumerable<ProductResponse>>(products);
        //    return result;
        //}                     // old one
        public async Task<PaginatedResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameters productQueryParameters)
        {
            var specs = new ProductWithTypeAndBrandSpecifications(productQueryParameters);

            var Repository = _unitOfWork.GetRepository<Product, int>();
            var products = await Repository.GetAllAsync(specs);
            //mapping from IEnumerable<Product> to IEnumerable<ProductResponse>
            var productsResult = _mapper.Map<IEnumerable<ProductResponse>>(products);
            var result = new PaginatedResponse<ProductResponse>()
            {
                Data = productsResult,
                PageIndex = productQueryParameters.PageIndex,
                PageSize = productQueryParameters.PageSize,
                TotalCount = productsResult.Count()
            };
            return result;
        }
        public async Task<IEnumerable<BrandResponse>> GetAllBrandsAsync()
        {
            var repository=_unitOfWork.GetRepository<ProductBrand,int>();
            var brands=await repository.GetAllAsync();
            //mapping from IEnumerable<ProductBrand> to IEnumerable<BrandResponse>
            var result=_mapper.Map<IEnumerable<BrandResponse>>(brands);
            return result;
        }
        public async Task<IEnumerable<TypeResponse>> GetAllTypesAsync()
        {
            var repository = _unitOfWork.GetRepository<ProductType, int>();
            var types = await repository.GetAllAsync();
            //mapping from IEnumerable<ProductType> to IEnumerable<TypeResponse>
            var result = _mapper.Map<IEnumerable<TypeResponse>>(types);
            return result;
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var specs=new ProductWithTypeAndBrandSpecifications(id);

            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specs);
            //mapping from Product to ProductResponse
            return _mapper.Map<ProductResponse>(product);
        }
    }
}

using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Shared.DataTransferObjects.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
                    // Source , Destination
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.BrandName, options =>
                options.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.TypeName, options =>
                options.MapFrom(src => src.ProductType.Name))
                .ForMember(dest=>dest.PictureUrl , options=>
                options.MapFrom<PictureUrlResolver>(/*src=>$"https://localhost:7092/{src.PictureUrl}"*/));

            CreateMap<ProductBrand, BrandResponse>();
            CreateMap<ProductType, TypeResponse>();
        }
    }

    public class PictureUrlResolver(IConfiguration _configuration) : IValueResolver<Product, ProductResponse, string>
    {
        public string Resolve(Product source, ProductResponse destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrWhiteSpace(source.PictureUrl))
            {
                return $"{_configuration["BaseUrl"]}{source.PictureUrl}";  // read the base url from the appsettings
            }
            return string.Empty ;
        }
    }
}

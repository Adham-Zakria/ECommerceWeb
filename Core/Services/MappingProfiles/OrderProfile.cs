using AutoMapper;
using Domain.Models.Orders;
using Domain.Models.Products;
using Microsoft.Extensions.Configuration;
using Shared.DataTransferObjects.Authentication;
using Shared.DataTransferObjects.Order;
using Shared.DataTransferObjects.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderAddress,AddressDto>().ReverseMap();

            CreateMap<OrderItems, OrderItemDto>()
                .ForMember(dest=>dest.PictureUrl ,option=>option.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.DeliveryMethod,
                opt=>opt.MapFrom(src=>src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.Total,
                opt => opt.MapFrom(src => src.DeliveryMethod.Price + src.SubTotal)); // total = order price + DeliveryMethod.Price

            CreateMap<DeliveryMethod, DeliveryMethodResponse>();
        }
    }

    public class OrderItemPictureUrlResolver(IConfiguration _configuration) : IValueResolver<OrderItems, OrderItemDto, string>
    {
        public string Resolve(OrderItems source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrWhiteSpace(source.PictureUrl))
            {
                return $"{_configuration["BaseUrl"]}{source.PictureUrl}";  // read the base url from the appsettings
            }
            return string.Empty;
        }
    }
}

using AutoMapper;
using Domain.Models.Identity;
using Shared.DataTransferObjects.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}

using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ServicesAbstraction;
using Shared.DataTransferObjects.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork , 
                                IMapper _mapper , IBasketRepository _basketRepository ,
                                UserManager<ApplicationUser> _userManager ,
                                IOptions<JWTOptions> _jwtOptions) : IServiceManager
    {
        private readonly Lazy<IProductService> _lazyProductService =
            new Lazy<IProductService>(()=>new ProductService(_unitOfWork , _mapper));

        private readonly Lazy<IBasketService> _lazyBasketService =
            new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));

        private readonly Lazy<IAuthenticationService> _lazyAuthenticationService =
            new Lazy<IAuthenticationService>(() => new AuthenticationService(_userManager, _jwtOptions , _mapper));

        public IProductService ProductService => _lazyProductService.Value;

        public IBasketService BasketService => _lazyBasketService.Value;

        public IAuthenticationService AuthenticationService => _lazyAuthenticationService.Value;
    }
}

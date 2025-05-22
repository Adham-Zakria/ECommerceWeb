using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.MappingProfiles;
using ServicesAbstraction;
using Shared.DataTransferObjects.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ApplicationServiceRegisteration
    {
        public static IServiceCollection AddApplicationService
            (this IServiceCollection services , IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            services.AddScoped<ICacheService, CacheService>();

            #region Service Manager
            //services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();

            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<Func<IProductService>>(provider => () 
              => provider.GetRequiredService<IProductService>());
            
            services.AddScoped<Func<IBasketService>>(provider => () 
              => provider.GetRequiredService<IBasketService>());
            
            services.AddScoped<Func<IAuthenticationService>>(provider => () 
              => provider.GetRequiredService<IAuthenticationService>());
            
            services.AddScoped<Func<IOrderService>>(provider => () 
              => provider.GetRequiredService<IOrderService>());

            services.AddScoped<Func<IPaymentService>>(provider => ()
              => provider.GetRequiredService<IPaymentService>());
            #endregion

            services.Configure<JWTOptions>(configuration.GetSection("JWTOptions"));
            return services;
        }
    }
}

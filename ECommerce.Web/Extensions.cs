using Domain.Contracts;
using ECommerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web
{
    public static class Extensions
    {
        public static IServiceCollection AddWebApplicationService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // Func<ActionContext, out IActionResult>
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
            });
            return services;
        }

        public static async Task InitializeDb(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
        }
    }
}

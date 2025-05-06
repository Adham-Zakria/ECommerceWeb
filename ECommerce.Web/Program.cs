using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using Persistence.Data;
using Services.MappingProfiles;
using ServicesAbstraction;
using Services;
using ECommerce.Web.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.ErrorModels;
using ECommerce.Web.Factories;

namespace ECommerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region Add services to the container

            //builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddWebApplicationService();
            //builder.Services.AddDbContext<StoreDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            //});
            //builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
            //builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddInfrastructureRegisteration(builder.Configuration);
            builder.Services.AddApplicationService();
            //builder.Services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    // Func<ActionContext, out IActionResult>
            //    options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
            //});
            #endregion

            var app = builder.Build();

            //await InitializeDb(app);
            await app.InitializeDb();

            #region Custom Middleware

            //app.Use(async (Context, Next) =>
            //{
            //    Console.WriteLine("Process Request");
            //    await Next.Invoke();
            //    Console.WriteLine("Response");
            //    Console.WriteLine(Context.Response);
            //});
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
            #endregion

            #region Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers(); 
            #endregion

            app.Run();
        }

        //public static async Task InitializeDb(WebApplication app)
        //{
        //    using var scope = app.Services.CreateScope();
        //    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        //    await dbInitializer.InitializeAsync();
        //}
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServicesAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    public class RedisCacheAttribute(int durationInSec = 1000) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            //1) Create the cache key
            var key = CreateCacheKey(context.HttpContext.Request);
            //2) Search by this key in redis
            var cacheValue = await cacheService.GetAsync(key);
            //2.1) Found => return the object without executing the end point
            //2.2) Notfound => executing the end point then take the result and store it in redis
            if (cacheValue is not null) 
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,
                };
            }

            var executedContext = await next.Invoke(); // execute the end point

            if (executedContext.Result is OkObjectResult res)
                await cacheService.SetAsync(key, res.Value!, TimeSpan.FromSeconds(durationInSec));
        }

        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(request.Path + "?"); // ? for query parameter

            foreach (var item in request.Query.OrderBy(q=>q.Key)) 
                builder.Append($"{item.Key}={item.Value}&");

            return builder.ToString().Trim('&');
        }
    }
}

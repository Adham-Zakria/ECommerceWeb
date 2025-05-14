using Domain.Exceptions;
using Shared.ErrorModels;
using System.Net;
using System.Text.Json;

namespace ECommerce.Web.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;
        public CustomExceptionHandlerMiddleware
            (RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                // if the end point is not found
                await HandleNotFoundEndPoint(context);
            }
            catch (Exception ex)
            {
                await HandleCatchException(context, ex);
            }
        }

        private async Task HandleCatchException(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "Something Went Wrong");

            // Response object with statue code and content type
            //Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails()
            {
                //StatusCode = (int)HttpStatusCode.InternalServerError,
                ErrorMessage = ex.Message
            };
            response.StatusCode = ex switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                BadRequestException badRequestExcep => GetValidationErrors(badRequestExcep , response),
                _ => (int)HttpStatusCode.InternalServerError, // default
            };

            context.Response.StatusCode = response.StatusCode;

            // convert the response into Json
            var jsonResult = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResult);
        }

        private int GetValidationErrors(BadRequestException badRequestExcep , ErrorDetails res)
        {
            res.Errors=badRequestExcep.Errors;
            return (int)HttpStatusCode.BadRequest;
        }

        private static async Task HandleNotFoundEndPoint(HttpContext context)
        {
            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                context.Response.ContentType = "application/json";
                var response = new ErrorDetails()
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    ErrorMessage = $"End point with this path : {context.Request.Path} is not found"
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}

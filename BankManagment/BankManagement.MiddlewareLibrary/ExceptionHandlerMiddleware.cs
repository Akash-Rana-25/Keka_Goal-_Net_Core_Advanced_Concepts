using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BankManagement.MiddlewareLibrary
{

    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500; // Internal Server Error

                // Return an error response to the client.
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    message = "An error occurred.",
                    exception = ex.Message
                }));
            }
        }
    }
}

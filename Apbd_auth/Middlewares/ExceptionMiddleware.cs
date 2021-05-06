using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Apbd_auth.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                HandleException(httpContext, ex);
            }
        }


        private void HandleException(HttpContext httpContext, Exception exception)
        {
            // Zapisywanie błędu do pliku logs.txt
        }

    }
}

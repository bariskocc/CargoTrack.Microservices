using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CargoTrack.Services.Identity.API.Infrastructure.Middlewares
{
    public class RequestPerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestPerformanceMiddleware> _logger;
        private const int PerformanceThresholdMs = 500;

        public RequestPerformanceMiddleware(RequestDelegate next, ILogger<RequestPerformanceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();
                var elapsedMilliseconds = sw.ElapsedMilliseconds;

                if (elapsedMilliseconds > PerformanceThresholdMs)
                {
                    var requestPath = context.Request.Path;
                    var requestMethod = context.Request.Method;
                    
                    _logger.LogWarning(
                        "Uzun süren istek tespit edildi - {RequestMethod} {RequestPath} - Süre: {ElapsedMilliseconds}ms",
                        requestMethod,
                        requestPath,
                        elapsedMilliseconds);
                }
            }
        }
    }
} 
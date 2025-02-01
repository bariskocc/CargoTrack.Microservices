using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CargoTrack.Services.Identity.API.Infrastructure.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bir hata oluştu: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new { message = exception.Message };

            switch (exception)
            {
                case UserNotFoundException:
                case RoleNotFoundException:
                case PermissionNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case DuplicateEmailException:
                case DuplicateUsernameException:
                case DuplicateRoleNameException:
                case DuplicatePermissionSystemNameException:
                case InvalidPasswordException:
                case UserLockedOutException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = new { message = "Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz." };
                    break;
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
} 
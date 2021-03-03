using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Files.Management.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Files.Management.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;
        private readonly IWebHostEnvironment environment;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment environment)
        {
            this.next = next;
            this.environment = environment;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (BusinessException ex) //Business Exceptions
            {
                logger.LogInformation(ex.Message);
                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    message = ex.Message,
                    token = ex.Token
                }));
            }
            catch (ValidationException ex) //Validation Exceptions
            {
                logger.LogInformation(ex.Message);
                httpContext.Response.StatusCode = 400;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    token = "validation-error",
                    message = ex.Errors.Select(error => new
                    {
                        property = error.PropertyName,
                        error = error.ErrorMessage
                    })
                }
                ));
            }
            catch (Exception ex) //Unhandled Exceptions
            {
                var guid = Guid.NewGuid().ToString();

                var errorLog = new StringBuilder();
                errorLog.AppendLine($"An error occurred (Guid = {guid}): {ex.Message}");
                errorLog.AppendLine($"Request Path: {httpContext.Request.Path}");
                errorLog.AppendLine($"Request Method: {httpContext.Request.Method}");
                errorLog.AppendLine($"Request Headers:");
                foreach (var header in httpContext.Request.Headers)
                {
                    errorLog.AppendLine($"\t{header.Key}:{header.Value}");
                }
                string body = new StreamReader(httpContext.Request.Body).ReadToEnd();
                errorLog.AppendLine($"Request Body: {body}");

                logger.LogError(errorLog.ToString());

                httpContext.Response.StatusCode = 500;
                httpContext.Response.Headers.Add("Content-Type", "application/json");

                if (environment.IsDevelopment())
                {
                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = ex.Message,
                        innerException = ex.InnerException,
                        stackTrace = ex.StackTrace,
                    }));
                }
                else
                {
                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        message = $"An error ocurred with Guid = {guid}. Contact the System Administration to fix this problem"
                    }));
                }

            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }

}

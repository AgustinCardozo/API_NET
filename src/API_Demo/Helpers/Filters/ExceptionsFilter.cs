using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace API_Demo.Helpers.Filters
{
    public class ExceptionsFilter : IExceptionFilter
    {
        private readonly ILogger logger;

        public ExceptionsFilter(ILogger logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            Exception ex = context.Exception;

            logger.LogError($"Error en {context.HttpContext.Request.Method} {context.HttpContext.Request.Path.Value}: {ex.Message}\n" +
                $"{ex.StackTrace}{(ex.InnerException != null ? $"\n\tInnerException: {ex.InnerException.Message}" : "")}");

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new JsonResult(ex);

            if (ex is UnauthorizedAccessException)
            {
                logger.LogError($"Unauthorized en {context.HttpContext.Request.Method} {context.HttpContext.Request.Path.Value}: {ex.Message}");
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult(ex);
            }
            else
            {
                logger.LogError($"Error en {context.HttpContext.Request.Method} {context.HttpContext.Request.Path.Value}: {ex.Message}\n" +
                    $"{ex.StackTrace}{(ex.InnerException != null ? $"\n\tInnerException: {ex.InnerException.Message}" : "")}");
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = new JsonResult(ex);
            }
        }
    }
}

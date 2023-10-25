using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace API_Demo.Helpers.Filters
{
    public class RequestsFilter : IActionFilter
    {
        private readonly ILogger logger;

        public RequestsFilter(ILogger logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string queryString = context.HttpContext.Request.QueryString.Value;
            string requestBody = JsonConvert.SerializeObject(context.ActionArguments, Formatting.Indented);
            JObject requestJson = JObject.Parse(requestBody);

            if (context.HttpContext.Request.Path.Value.Contains("/api/auth") && requestJson["user"]?["password"] != null)
            {
                requestJson["user"]["password"] = "*****";
            }

            requestBody = requestJson.ToString(Formatting.Indented);

            logger.LogInformation($"{context.HttpContext.User?.Identity?.Name} - {context.HttpContext.Request.Method} {context.HttpContext.Request.Path.Value}" +
                $"{(!String.IsNullOrWhiteSpace(queryString) ? $"{queryString}" : "")}" +
                $"{(!String.IsNullOrWhiteSpace(requestBody) ? $" - REQ BODY: {requestBody}" : "")}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null) { return; }

            try
            {
                string statusCode = context.HttpContext.Response.StatusCode.ToString();
                var result = context.Result;
                string response = "";
                if (result is JsonResult json)
                {
                    var x = json.Value;
                    response = JsonConvert.SerializeObject(x, Formatting.Indented);
                }
                if (result is ObjectResult obj)
                {
                    var x = obj.Value;
                    response = JsonConvert.SerializeObject(x, Formatting.Indented);
                }
                logger.LogInformation($"{context.HttpContext.User?.Identity?.Name} - {context.HttpContext.Request.Method} {context.HttpContext.Request.Path.Value}" +
                    $"{(!String.IsNullOrWhiteSpace(statusCode) || !String.IsNullOrWhiteSpace(response) ? $" - RESPONSE: {statusCode} - {response}" : "")}");
            }
            catch (Exception ex)
            {
                logger.LogError($"No se pudo loggear la respuesta del request: {context.HttpContext.Request.Path} - Error: {ex.Message}");
            }
        }
    }
}

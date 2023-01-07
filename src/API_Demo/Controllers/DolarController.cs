using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace API_Demo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cotizador")]
    public class DolarController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger logger;

        public DolarController(IConfiguration configuration, ILogger<DolarController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dolar-oficial"), Authorize(Roles = Consts.ADMIN)]
        public async Task<IActionResult> GetCotizacionDolarOficial()
        {
            return await GetCotizacion(Consts.ConfigKeys.DOLAR_OFICIAL);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dolar-blue"), Authorize(Roles = Consts.ADMIN)]
        public async Task<IActionResult> GetCotizacionDolarBlue()
        {
            return await GetCotizacion(Consts.ConfigKeys.DOLAR_BLUE);
        }

        private async Task<IActionResult> GetCotizacion(string url)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetStringAsync(configuration.GetSection(url).Value);
                    var content = JsonConvert.DeserializeObject<DolarRes>(response);
                    return Ok(content);
                }
                catch (Exception ex)
                {
                    string exceptionMessage = $"Error: {ex.Message} {(ex.InnerException != null ? $" - InnerException: " + ex.InnerException.Message : "")} - StackTrace: {ex.StackTrace}";
                    logger.LogError(exceptionMessage);
                    return Problem(exceptionMessage);
                }
            }
        }
    }
}

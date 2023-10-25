using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace API_Demo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/cotizador")]
    [ApiVersion("1.0")]
    public class CotizadorController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger logger;

        public CotizadorController(IConfiguration configuration, ILogger<CotizadorController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dolar-oficial"), Authorize(Roles = Consts.ADMIN)]
        public async Task<IActionResult> GetCotizacionDolarOficial()
        {
            return await GetCotizacion<DolarRes>(Consts.ConfigKeys.URL.DOLAR_OFICIAL);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dolar-blue"), Authorize(Roles = Consts.ADMIN)]
        public async Task<IActionResult> GetCotizacionDolarBlue()
        {
            return await GetCotizacion<DolarRes>(Consts.ConfigKeys.URL.DOLAR_BLUE);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCotizaciones()
        {
            return await GetCotizacion<Cotizador>(Consts.ConfigKeys.URL.COTIZADOR);
        }

        private async Task<IActionResult> GetCotizacion<T>(string url)
        {
            using (var httpClient = new HttpClient())
            {
                try
                    {
                    var content = await httpClient.GetFromJsonAsync<T>(configuration.GetSection(url).Value);
                    return Ok(content);
                }
                catch (Exception ex)
                {
                    logger.LogError(ErrorMessage.GetException(ex));
                    return Problem(ErrorMessage.GetException(ex));
                }
            }
        }
    }
}

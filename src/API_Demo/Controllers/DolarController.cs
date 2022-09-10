using API_Demo.Helpers;
using API_Demo.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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

        public DolarController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        //[AllowAnonymous]
        [HttpGet]
        [Route("dolar-oficial"), Authorize(Roles = Consts.ADMIN)]
        public async Task<IActionResult> GetCotizacionDolarOficial()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(configuration.GetSection("URL:dolar_oficial").Value);
                if (!response.IsSuccessStatusCode)
                    return NotFound();
                var content = JsonConvert.DeserializeObject<DolarRes>(await response.Content.ReadAsStringAsync());
                return Ok(content);
            }
        }

        //[AllowAnonymous]
        [HttpGet]
        [Route("dolar-blue"), Authorize(Roles = Consts.ADMIN)]
        public async Task<IActionResult> GetCotizacionDolarBlue()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(configuration.GetSection("URL:dolar_blue").Value);
                var content = JsonConvert.DeserializeObject<DolarRes>(response);
                //content.fecha = Convert.ToDateTime(content.fecha.ToString("yyyy-MM-dd HH:mm:ss"));
                return Ok(content);
            }
        }
    }
}

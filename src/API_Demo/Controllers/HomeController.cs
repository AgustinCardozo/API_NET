using Carter;
using Carter.Response;
using Microsoft.Extensions.Logging;

namespace API_Demo.Controllers
{
    public class HomeController : CarterModule
    {
        public HomeController(ILogger<HomeController> logger)
        {
            Get("/api", async (req, res) =>
            {
                logger.LogInformation("Se inicia la API");
                await res.AsJson("Bienvenido!");
            });
        }
    }
}

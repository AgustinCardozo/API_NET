using Carter;
using Carter.Response;

namespace Controller.Controllers
{
    public class HomeController : CarterModule
    {
        public HomeController()
        {
            Get("/API", async (req, res) =>
            {
                await res.AsJson("Bienvenido!");
            });
        }
    }
}

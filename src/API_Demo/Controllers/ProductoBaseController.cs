using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API_Demo.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/productos")]
    public abstract class ProductoBaseController : ControllerBase
    {
        protected readonly IProductoRepository productoRepository;

        protected ProductoBaseController(IProductoRepository productoRepository)
        {
            this.productoRepository = productoRepository;
        }

        [HttpGet]
        public abstract Task<IActionResult> GetProductos([FromQuery] PageReq page);
    }
}

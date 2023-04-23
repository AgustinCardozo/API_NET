using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace API_Demo.Controllers
{
    [ApiController]
    [Route("API/productos")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepository productoRepository;

        public ProductoController(IProductoRepository productoRepository)
        {
            this.productoRepository = productoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductos([FromQuery]PageReq page)
        {
            return Ok(await productoRepository.GetProductosPaginados(page));
        }

        [HttpGet("paginacion")]
        public async Task<IActionResult> GetProductosPaginados([FromQuery] PageReq page)
        {
            var productos = await productoRepository.GetProductos();
            var response = productos.Skip((page.pagina - 1) * page.cantidadRegistroPorPagina).Take(page.cantidadRegistroPorPagina);
            return Ok(response);
        }
    }
}

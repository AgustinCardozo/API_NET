using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace API_Demo.Controllers.V1
{
    [ApiVersion("1.0")]
    public class ProductoController : ProductoBaseController
    {
        public ProductoController(IProductoRepository productoRepository) : base(productoRepository) { }

        [MapToApiVersion("1.0")]
        public override async Task<IActionResult> GetProductos([FromQuery] PageReq page)
        {
            return Ok(await productoRepository.GetProductosPaginados(page));
        }

        [HttpGet("paginacion")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetProductosPaginados([FromQuery] PageReq page)
        {
            var productos = await productoRepository.GetProductos();
            var response = productos.Skip((page.pagina - 1) * page.cantidadRegistroPorPagina).Take(page.cantidadRegistroPorPagina);
            return Ok(response);
        }
    }
}

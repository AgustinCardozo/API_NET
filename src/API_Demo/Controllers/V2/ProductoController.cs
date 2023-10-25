using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API_Demo.Controllers.V2
{
    public class ProductoController : ProductoBaseController
    {
        public ProductoController(IProductoRepository productoRepository) : base(productoRepository) { }

        [MapToApiVersion("2.0")]
        public override async Task<IActionResult> GetProductos([FromQuery] PageReq page)
        {
            var resultado = await productoRepository.GetProductos(page.like, page.error, page.campo);
            return Ok(resultado);
        }
    }
}

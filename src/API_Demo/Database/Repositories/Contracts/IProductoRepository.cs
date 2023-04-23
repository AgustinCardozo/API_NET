using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Demo.Database.Repositories.Contracts
{
    public interface IProductoRepository
    {
        Task<IEnumerable<ProductoRes>> GetProductos();
        Task<IEnumerable<ProductoRes>> GetProductosPaginados(PageReq page);
    }
}

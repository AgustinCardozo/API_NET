using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Demo.Database.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly DapperContext dapperContext;

        public ProductoRepository(DapperContext dapperContext)
        {
            this.dapperContext = dapperContext;
        }

        public async Task<IEnumerable<ProductoRes>> GetProductos()
        {
            using (var connection = dapperContext.CreateConnection())
            {
                var query = $"select prod_codigo, ltrim(rtrim(prod_detalle)) 'prod_detalle', prod_precio, prod_familia, prod_rubro from GD2015C1.dbo.Producto ";

                return await connection.QueryAsync<ProductoRes>(query);
            }
        }

        public async Task<IEnumerable<ProductoRes>> GetProductosPaginados(PageReq page)
        {
            using (var connection = dapperContext.CreateConnection())
            {
                var query = $"select prod_codigo, ltrim(rtrim(prod_detalle)) 'prod_detalle', prod_precio, prod_familia, prod_rubro " +
                    $"from GD2015C1.dbo.Producto " +
                    $"ORDER BY 1 " +
                    $"OFFSET @Offset ROWS " +
                    $"FETCH NEXT @Next ROWS ONLY";

                return await connection.QueryAsync<ProductoRes>(
                        sql: query,
                        param: new { Offset = page.pagina, Next = page.cantidadRegistroPorPagina }
                    );
            }
        }
    }
}

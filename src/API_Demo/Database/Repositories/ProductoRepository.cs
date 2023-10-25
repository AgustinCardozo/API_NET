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

        public async Task<IEnumerable<ProductoRes>> GetProductos(bool like = true, string error = "", string campo = "")
        {
            using (var connection = dapperContext.CreateConnection())
            {
                string filtroWhere = like ? $"WHERE {campo} LIKE \'%{error.ToUpper().Trim()}%\' " : $"WHERE {campo} NOT LIKE \'%{error.ToUpper().Trim()}%\' ";
                var query = $"SELECT prod_codigo, ltrim(rtrim(prod_detalle)) 'prod_detalle', prod_precio, prod_familia, prod_rubro " +
                    $"FROM GD2015C1.dbo.Producto " +
                    $"{(!string.IsNullOrEmpty(error) ? filtroWhere : null)}";

                return await connection.QueryAsync<ProductoRes>(query);
            }
        }

        public async Task<IEnumerable<ProductoRes>> GetProductosPaginados(PageReq page)
        {
            using (var connection = dapperContext.CreateConnection())
            {
                var query = $"SELECT prod_codigo, ltrim(rtrim(prod_detalle)) 'prod_detalle', prod_precio, prod_familia, prod_rubro " +
                    $"FROM GD2015C1.dbo.Producto " +
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

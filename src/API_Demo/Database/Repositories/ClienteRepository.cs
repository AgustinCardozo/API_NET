using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API_Demo.Database.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly DapperContext dapperContext;

        public ClienteRepository(DapperContext dapperContext)
        {
            this.dapperContext = dapperContext;
        }

        public async Task<string> CreateAsync(ClienteReq clienteReq)
        {
            using var connection = dapperContext.CreateConnection();

            var clienteParams = new
            {
                id_cliente = clienteReq.idCliente,
                razon_social = clienteReq.razonSocial,
                telefono = clienteReq.telefono,
                domicilio = clienteReq.domicilio,
                limite = clienteReq.limite,
                id_vendedor = clienteReq.idVendedor
            };

            return await connection.ExecuteScalarAsync<string>(
                sql: Consts.StoreProcName.CreateCliente,
                param: clienteParams,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<string> DeleteAsync(string idCliente)
        {
            using var connection = dapperContext.CreateConnection();
            var clienteParam = new { id_cliente = idCliente };
            return await connection.ExecuteScalarAsync<string>(
                sql: Consts.StoreProcName.DeleteCliente,
                param: clienteParam,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<ClienteRes>> GetAllAsync()
        {
            using var connection = dapperContext.CreateConnection();
            var result = await connection
                .QueryAsync<ClienteRes>(
                    sql: Consts.StoreProcName.GetClientes,
                    param: new { id_cliente = (string)null },
                    commandType: CommandType.StoredProcedure
                );
            return result.ToList();
        }

        public async Task<ClienteRes> GetByIdAsync(string idCliente)
        {
            using var connection = dapperContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ClienteRes>(
                sql: Consts.StoreProcName.GetClientes,
                param: new { id_cliente = idCliente },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<string> UpdateAsync(ClienteReq clienteReq)
        {
            using var connection = dapperContext.CreateConnection();

            var clienteParams = new
            {
                id_cliente = clienteReq.idCliente,
                razon_social = clienteReq.razonSocial,
                telefono = clienteReq.telefono,
                domicilio = clienteReq.domicilio,
                limite = clienteReq.limite,
                id_vendedor = clienteReq.idVendedor
            };

            return await connection.ExecuteScalarAsync<string>(
                sql: Consts.StoreProcName.UpdateCliente,
                param: clienteParams,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

using Dapper;
using Model.Request;
using Model.Response;
using Repository.Contracts;
using Repository.Database;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly DapperContext dapperContext;

        public ClienteRepository(DapperContext dapperContext)
        {
            this.dapperContext = dapperContext;
        }

        public async Task<string> DeleteCliente(string idCliente)
        {
            using(var connection = dapperContext.CreateConnection())
            {
                var clienteParam = new { id_cliente = idCliente };
                return await connection.ExecuteScalarAsync<string>(StoreProcName.DeleteCliente, clienteParam, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<List<ClienteRes>> GetClientes(string idCliente = null)
        {
            using(var connection = dapperContext.CreateConnection())
            {
                var clienteReq = new ClienteReq();

                if (idCliente != null)
                    clienteReq = new ClienteReq { idCliente = idCliente };

                var clienteParam = new { id_cliente = clienteReq.idCliente };
                var data = await connection.QueryAsync<ClienteRes>(StoreProcName.GetClientes, clienteParam, commandType: CommandType.StoredProcedure);

                return data.ToList();
            }
        }

        public async Task<string> SetCliente(ClienteReq clienteReq, string httpMethod)
        {
            using (var connection = dapperContext.CreateConnection())
            {
                var clienteParams = new
                {
                    id_cliente = clienteReq.idCliente,
                    razon_social = clienteReq.razonSocial,
                    telefono = clienteReq.telefono,
                    domicilio = clienteReq.domicilio,
                    limite = clienteReq.limite,
                    id_vendedor = clienteReq.idVendedor
                };

                if(httpMethod.ToUpper() == "CREATE")
                    return await connection.ExecuteScalarAsync<string>(StoreProcName.CreateCliente, clienteParams, commandType: CommandType.StoredProcedure);
                else
                    return await connection.ExecuteScalarAsync<string>(StoreProcName.UpdateCliente, clienteParams, commandType: CommandType.StoredProcedure);

            }
        }
    }
}

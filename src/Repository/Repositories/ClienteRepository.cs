using Dapper;
using Database;
using Model.Request;
using Model.Response;
using Repository.Contracts;
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

        public async Task<List<ClienteRes>> GetClientes(string idCliente = null)
        {
            using(var connection = dapperContext.CreateConnection())
            {
                var clienteReq = new ClienteReq { id_cliente = null };

                if (idCliente != null)
                    clienteReq = new ClienteReq { id_cliente = idCliente };

                var clienteParam = new { id_cliente = clienteReq.id_cliente };
                var data = await connection.QueryAsync<ClienteRes>(StoreProcName.GetClientes, clienteParam, commandType: CommandType.StoredProcedure);

                return data.ToList();
            }
        }
    }
}

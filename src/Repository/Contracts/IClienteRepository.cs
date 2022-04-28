using Model.Request;
using Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IClienteRepository
    {
        Task<List<ClienteRes>> GetClientes(string idCliente = null);
        Task<string> DeleteCliente(string idCliente);
        Task<string> SetCliente(ClienteReq clienteReq, string httpMethod);
    }
}

using API_Demo.Models.Requests;
using API_Demo.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Demo.Repositories.Contracts
{
    public interface IClienteRepository
    {
        Task<List<ClienteRes>> GetClientes(string idCliente = null);
        Task<string> DeleteCliente(string idCliente);
        Task<string> SetCliente(ClienteReq clienteReq, string httpMethod);
    }
}

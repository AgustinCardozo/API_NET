using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Demo.Services.Contracts
{
    public interface IClienteService
    {
        Task<string> DeleteAsync(string idCliente);
        Task<List<ClienteRes>> GetAllAsync();
        Task<ClienteRes> GetByIdAsync(string idCliente);
        Task<string> SetClienteAysnc(ClienteReq clienteReq, string httpMethod);
    }
}
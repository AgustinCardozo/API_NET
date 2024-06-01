using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Demo.Database.Repositories.Contracts
{
    public interface IClienteRepository
    {
        Task<string> CreateAsync(ClienteReq clienteReq);
        Task<string> DeleteAsync(string idCliente);
        Task<List<ClienteRes>> GetAllAsync();
        Task<ClienteRes> GetByIdAsync(string idCliente);
        Task<string> UpdateAsync(ClienteReq clienteReq);
    }
}

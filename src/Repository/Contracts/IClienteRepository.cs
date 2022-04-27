using Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IClienteRepository
    {
        Task<List<ClienteRes>> GetClientes(string idCliente = null);
    }
}

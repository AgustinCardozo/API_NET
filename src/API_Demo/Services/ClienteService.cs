using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Demo.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository clienteRepo;

        public ClienteService(IClienteRepository clienteRepo)
        {
            this.clienteRepo = clienteRepo;
        }

        public async Task<string> DeleteAsync(string idCliente)
        {
             return await clienteRepo.DeleteAsync(idCliente);
        }

        public async Task<List<ClienteRes>> GetAllAsync()
        {
            return await clienteRepo.GetAllAsync();
        }

        public async Task<ClienteRes> GetByIdAsync(string idCliente)
        {
            return await clienteRepo.GetByIdAsync(idCliente);
        }

        public async Task<string> SetClienteAysnc(ClienteReq clienteReq, string httpMethod)
        {
            return httpMethod switch
            {
                Consts.HttpMethods.POST => await clienteRepo.CreateAsync(clienteReq),
                Consts.HttpMethods.PUT => await clienteRepo.UpdateAsync(clienteReq),
                _ => "Invalid HTTP Method",
            };
        }
    }
}
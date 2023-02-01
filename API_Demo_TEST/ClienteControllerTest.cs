using API_Demo.Controllers;
using API_Demo.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace API_Demo_TEST
{
    public class ClienteControllerTest : CommonTest
    {
        private ClienteController clienteController;

        protected override void InitServices()
        {
            clienteController = (ClienteController)scope.ServiceProvider.GetService(typeof(ClienteController));
        }

        [Test]
        public async Task GetClientes_Test()
        {
            var response = (ObjectResult)await clienteController.GetClientes();
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK);
        }

        [Test]
        public async Task UpdateClienteInvadio()
        {
            var req = new ClienteReq
            {
                domicilio = "Darwin 747"
            };

            var response = (StatusCodeResult)await clienteController.UpdateCliente(req);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status500InternalServerError);
        }
    }
}

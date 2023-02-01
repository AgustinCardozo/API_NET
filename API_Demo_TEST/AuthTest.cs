using API_Demo.Controllers;
using API_Demo.Models.Requests;
using API_Demo.Services.Contracts;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace API_Demo_TEST
{
    public class AuthTest : CommonTest
    {
        private ILogginService logginService;
        private IConfiguration configuration;
        private AuthController authController;

        protected override void InitServices()
        {
            configuration = (IConfiguration)scope.ServiceProvider.GetService(typeof(IConfiguration));
            logginService = (ILogginService)scope.ServiceProvider.GetService(typeof(ILogginService));
            authController = (AuthController)scope.ServiceProvider.GetService(typeof(AuthController));
        }

        [Test]
        public void RegistrarUsuarioExiste_Test()
        {
            var nuevoUsuario = new RegistrarUsuarioReq
            {
                usuario = "acardozo"
            };

            var exception = Assert.Throws<Exception>(() => logginService.RegistrarUsuario(nuevoUsuario));
            Assert.That(exception.Message, Is.EqualTo("Ya existe el usuario"));
        }
    }
}

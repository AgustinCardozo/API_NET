using API_Demo.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RestSharp;

namespace API_Demo_TEST
{
    public class CotizadorControllerTest : CommonTest
    {
        //private static IServiceScope scope;
        //ClienteHelper clienteHelper;
        private CotizadorController cotizador;
        private IConfiguration configuration;

        //[SetUp]
        //public void Setup()
        //{
        //    ServiceProvider serviceProvider = ServiceProviderHelper.GenerateServiceProvider();
        //    scope = serviceProvider.CreateScope();

        //    //cotizador = (CotizadorController)scope.ServiceProvider.GetService(typeof(CotizadorController));
        //    //configuration = (IConfiguration)scope.ServiceProvider.GetService(typeof(IConfiguration));

        //    //var clienteDB = new Mock<IClienteRepository>();
        //    //clienteHelper = new ClienteHelper(clienteDB.Object);
        //}

        //[TearDown]
        //public void TearDown()
        //{
        //    scope.Dispose();
        //}
        
        protected override void InitServices()
        {
            cotizador = (CotizadorController)scope.ServiceProvider.GetService(typeof(CotizadorController));
            configuration = (IConfiguration)scope.ServiceProvider.GetService(typeof(IConfiguration));
        }

        [Test]
        public async Task Cotizador_Test()
        {
            var response = (ObjectResult)await cotizador.GetCotizaciones();
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK);
        }

        [TestCase("URL:cotizador")]
        [TestCase("URL:dolar_oficial")]
        [TestCase("URL:dolar_blue")]
        public async Task Conexion_TestAsync(string url)
        {
            //using var httpClient = new HttpClient();
            //var response = await httpClient.GetStringAsync(configuration.GetValue<string>("URL:cotizador"));
            //Assert.IsNotNull(response);
            var client = new RestClient();

            var request = new RestRequest(configuration.GetValue<string>(url), Method.Get);
            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                Assert.IsFalse(response.IsSuccessful);
                return;
            }

            Assert.IsTrue(response.IsSuccessful);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

    }
}

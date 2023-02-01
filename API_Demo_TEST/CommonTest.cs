using API_Demo_TEST.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace API_Demo_TEST
{
    [TestFixture]
    public abstract class CommonTest
    {
        protected static IServiceScope scope;
        //ClienteHelper clienteHelper;
        //CotizadorController cotizador;
        //IConfiguration configuration;

        [SetUp]
        public void Setup()
        {
            ServiceProvider serviceProvider = ServiceProviderHelper.GenerateServiceProvider();
            scope = serviceProvider.CreateScope();

            InitServices();

            //var clienteDB = new Mock<IClienteRepository>();
            //clienteHelper = new ClienteHelper(clienteDB.Object);
        }

        [TearDown]
        public void TearDown()
        {
            scope.Dispose();
        }

        protected abstract void InitServices();
    }
}

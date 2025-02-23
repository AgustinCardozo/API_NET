using API_Demo.Database;
using API_Demo.Database.Repositories;
using NUnit.Framework;

namespace API_Demo_TEST
{
    public class UsuarioRepositoryTest : CommonTest
    {
        //private static IServiceScope scope;
        //ClienteHelper clienteHelper;
        private DapperContext dapperContext;
        private UsuarioRepository userRepo;

        //[SetUp]
        //public void Setup()
        //{
        //    ServiceProvider serviceProvider = ServiceProviderHelper.GenerateServiceProvider();
        //    scope = serviceProvider.CreateScope();

        //    dapperContext = (DapperContext)scope.ServiceProvider.GetService(typeof(DapperContext));
        //    userRepo = new UsuarioRepository(dapperContext);

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
            dapperContext = (DapperContext)scope.ServiceProvider.GetService(typeof(DapperContext));
            userRepo = new UsuarioRepository(dapperContext);
        }

        [Test]
        public void GetUsuarios_Test()
        {
            //var clienteDB = new Mock<IClienteRepository>();
            //var clienteDB = new Mock<IUsuarioRepository>();
            ////clienteHelper = new ClienteHelper(clienteDB.Object);
            //clienteDB.Setup(user => user.GetUsuario("acardozo"))
            //    .Returns(new UsuarioRes
            //    {
            //        id = 5,
            //        usuario = "acardozo",
            //        password = "*****",
            //        mail = "acardozo@mail.com",
            //        nombre = "Agustin Cardozo",
            //        rol = "ADMIN",
            //        createdAt = new DateTime(2023, 01, 24, 17, 45, 46),
            //        updatedAt = null,
            //        deletedAt = null
            //    });
            //var clientes = clienteHelper.GetClientes();
            //Assert.IsNotNull(clientes);
            var users =  userRepo.GetUsuarios();
            Assert.IsNotNull(users, "Debe contener usuarios");
            Assert.IsTrue(users.Count == 4, "La cantidad de usuarios debe ser 4");
        }

        [TestCase("")]
        [TestCase("acardozo")]
        public void GetUsuario_Test(string username)
        {
            var user = userRepo.GetUsuario(username);

            if(user == null)
            {
                Assert.IsNull(user);
                return;
            }

            Assert.IsNotNull(user, "Debe existir el usuario");
            Assert.IsTrue(user.usuario == username, "El nombre de usuario debe ser 'acardozo'");
        }

        //[Test]
        //public void InsertarUsuarioInvalido_Test()
        //{
        //    var user = new RegistrarUsuarioReq
        //    {
        //        usuario = string.Empty,
        //        password = null,
        //        mail = "lala",
        //        nombre = null
        //    };

        //    var exception = Assert.Throws<LogginInvalidoException>(() => userRepo.InsertarUsuario(user));
        //    Assert.That(exception.Message, Is.EqualTo("Datos de registros incorrectos"));
        //}
    }
}
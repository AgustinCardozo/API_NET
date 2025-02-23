using API_Demo.Configurations;
using API_Demo.Helpers.Exceptions;
using API_Demo.Services;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace API_Demo_TEST
{
    public class ServicesTest : CommonTest
    {
        const string PASSWORD = "test_password";
        //IConfiguration configuration;
        IOptions<ApiDemoOptions> options;

        protected override void InitServices()
        {
            //configuration = (IConfiguration)scope.ServiceProvider.GetService(typeof(IConfiguration));
            options = (IOptions<ApiDemoOptions>)scope.ServiceProvider.GetService(typeof(IOptions<ApiDemoOptions>));
        }

        [Test]
        public void MD5Services_Test()
        {
            string pass = PASSWORD;
            var hash = options.Value.Hash; //configuration.GetValue<string>("hash");
            Assert.IsNotNull(hash);

            pass = MD5Service.Encrypt(pass,hash);
            Assert.IsFalse(pass == PASSWORD);


            pass = MD5Service.Decrypt(pass,hash);
            Assert.IsTrue(pass == PASSWORD);
        }

        [TestCase("","")]
        [TestCase(null, PASSWORD)]
        [TestCase("test", null)]
        public void ValidacionDeUsuarioYPassword_Test(string user, string pass)
        {
            var exception = Assert.Throws<UsuarioInvalidoException>(() => ValidationService.ValidacionDeUsuarioYPassword(user, pass));
            Assert.That(exception.Message, Is.EqualTo("Nombre de Usuario o Contraseña incorrectos."));
        }

        [TestCase("test")]
        [TestCase("1234567890")]
        public void ValidacionDeSeguridad_Test(string pass)
        {
            string path = "../../../../src/API_Demo/Assets/10k-most-common-passwords.txt";
            var exception = Assert.Throws<PasswordInvalidoException>(() => ValidationService.ValidacionDeSeguridad(pass, path));
            //Assert.That(exception.Message, Is.EqualTo("CONTRASEÑA INVÁLIDA: Longitud inválidad."));
            //Assert.That(exception.Message, Is.EqualTo("CONTRASEÑA INVÁLIDA: Debe seleccionar uno mas seguro."));
            var esValido = exception.Message.Contains("CONTRASEÑA INVÁLIDA: Longitud inválidad.") 
                || exception.Message.Contains("CONTRASEÑA INVÁLIDA: Debe seleccionar uno mas seguro.");
            Assert.IsTrue(esValido);
        }
    }
}

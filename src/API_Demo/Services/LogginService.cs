using API_Demo.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;

namespace API_Demo.Services
{
    public class LogginService : ILogginService
    {
        private readonly IJwtTokenService jwtTokenService;
        private readonly IUsuarioRepository usuarioRepository;
        //private readonly IConfiguration configuration;
        private readonly IOptions<ApiDemoOptions> options;
        private string hash;

        public LogginService(
            IJwtTokenService jwtTokenService, 
            IUsuarioRepository usuarioRepository, 
            IOptions<ApiDemoOptions> options, 
            IConfiguration configuration)
        {
            this.jwtTokenService = jwtTokenService;
            this.usuarioRepository = usuarioRepository;
            //this.configuration = configuration;
            this.options = options;
            hash = options.Value.Hash; //configuration.GetValue<string>("Hash");
        }

        public string RegistrarUsuario(RegistrarUsuarioReq usuario)
        {
            if (usuarioRepository.GetUsuario(usuario.usuario) != null)
            {
                throw new Exception("Ya existe el usuario");
            }

            var result = new UsuarioValidator().Validate(usuario);
            if (!result.IsValid)
            {
                var errors = result.Errors.ToList();
                var errorMsg = new StringBuilder();
                errorMsg.AppendLine("Datos de registros incorrectos: ");
                errors.ForEach(error => errorMsg.AppendLine($"\t{error}"));
                throw new LogginInvalidoException(errorMsg.ToString());
            }

            ValidationService.ValidacionDeSeguridad(usuario.password);
            string pass = usuario.password;
            usuario.password = MD5Service.Encrypt(usuario.password, hash);

            usuarioRepository.InsertarUsuario(usuario);

            var usuarioLoggin = new UsuarioLogginReq
            {
                username = usuario.usuario,
                password = pass
            };
            return IniciarSeccion(usuarioLoggin);
        }

        public string IniciarSeccion(UsuarioLogginReq usuario)
        {
            ValidationService.ValidacionDeUsuarioYPassword(usuario.username, usuario.password);
            UsuarioRes user = usuarioRepository.GetUsuario(usuario.username);

            if (user is null)
            {
                throw new UsuarioInvalidoException("No existe el usuario");
            }

            if (usuario.password != MD5Service.Decrypt(user.password, hash))
            {
                throw new PasswordInvalidoException("CONTRASEÑA INVÁLIDA: No coincide la contraseña");
            }

            return jwtTokenService.Authenticate(user);
        }

        public void RestaurarPassword(string username, string nuevoPass)
        {
            UsuarioRes user = usuarioRepository.GetUsuario(username);

            if (user is null)
            {
                throw new UsuarioInvalidoException("No existe el usuario");
            }

            ValidationService.ValidacionDeSeguridad(nuevoPass);
            nuevoPass = MD5Service.Encrypt(nuevoPass, hash);
            usuarioRepository.ModificarPassUsuario(user.id, nuevoPass);
        }
    }
}

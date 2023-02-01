using Microsoft.Extensions.Configuration;
using System;

namespace API_Demo.Services
{
    public class LogginService : ILogginService
    {
        private readonly IJwtTokenService jwtTokenService;
        private readonly IUsuarioRepository usuarioRepository;
        private readonly IConfiguration configuration;
        private string hash;

        public LogginService(IJwtTokenService jwtTokenService, IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            this.jwtTokenService = jwtTokenService;
            this.usuarioRepository = usuarioRepository;
            this.configuration = configuration;
            hash = configuration.GetValue<string>("Hash");
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
                throw new LogginInvalidoException("Datos de registros incorrectos");
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

            if (user == null)
            {
                throw new Exception("No existe el usuario");
            }

            if (usuario.password != MD5Service.Decrypt(user.password, hash))
            {
                throw new Exception("Contraseña invalida");
            }

            return jwtTokenService.Authenticate(user);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace API_Demo.Controllers
{
    [Route("api/usuarios"), Authorize(Roles = Consts.ADMIN)]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            this.usuarioRepository = usuarioRepository;
        }

        [AllowAnonymous]
        [HttpGet, Route("")]
        public IActionResult GetUsers()
        {
            var users = usuarioRepository.GetUsuarios();
            var usersList = new List<UsuarioRes>();

            foreach (var user in users)
            {
                var requestBody = PasswordHelper.HideUserPassword(user);
                usersList.Add(JsonConvert.DeserializeObject<UsuarioRes>(requestBody));
            }
            
            return Ok(usersList);
        }

        [AllowAnonymous]
        [HttpGet, Route("by-name/{username}")]
        public IActionResult GetUserByName(string username)
        {
            var user = usuarioRepository.GetUsuario(username);

            if(user == null)
            {
                return NotFound($"No se encontro al usuario {username}");
            }

            var requestBody = PasswordHelper.HideUserPassword(user);
            return Ok(requestBody);
        }

        [HttpPost, Route("insert")]
        public void InsertUser(RegistrarUsuarioReq usuarioReq)
        {
            usuarioRepository.InsertarUsuario(usuarioReq);
        }
    }
}

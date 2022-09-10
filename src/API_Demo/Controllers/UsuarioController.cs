using API_Demo.Database.Repositories.Contracts;
using API_Demo.Helpers;
using API_Demo.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet, Route("")]
        public IActionResult GetAllUsers()
        {
            return Ok(usuarioRepository.GetUsuarios());
        }

        [HttpGet, Route("by-name/{username}")]
        public IActionResult GetUserByName(string username)
        {
            return Ok(usuarioRepository.GetUsuario(username));
        }

        [HttpPost, Route("insert")]
        public void InsertUser(RegistrarUsuarioReq usuarioReq)
        {
            usuarioRepository.InsertarUsuario(usuarioReq);
        }
    }
}

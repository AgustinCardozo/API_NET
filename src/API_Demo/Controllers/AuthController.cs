using API_Demo.Helpers;
using API_Demo.Models.Requests;
using API_Demo.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace API_Demo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly ILogginService logginService;

        public AuthController(ILogger<AuthController> logger, ILogginService logginService)
        {
            this.logger = logger;
            this.logginService = logginService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("iniciar-seccion")]
        public IActionResult Login(UsuarioLogginReq user)
        {
            try
            {
                logger.LogInformation("Generando token de inicio de seccion");
                string token = logginService.IniciarSeccion(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error: {ex.Message} {(ex.InnerException != null ? $" - InnerException: " + ex.InnerException.Message : "")} - StackTrace: {ex.StackTrace}";
                logger.LogError(exceptionMessage);
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registrar-usuario")]
        public IActionResult UserRegister([FromBody] RegistrarUsuarioReq user)
        {
            try
            {
                logger.LogInformation("Generando token de registro de nuevo usuario");
                string token = logginService.RegistrarUsuario(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error: {ex.Message} {(ex.InnerException != null ? $" - InnerException: " + ex.InnerException.Message : "")} - StackTrace: {ex.StackTrace}";
                logger.LogError(exceptionMessage);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("user"), Authorize(Roles = Consts.ADMIN)]
        public IActionResult GetUsuario()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = User.FindFirstValue(ClaimTypes.Name);
            var mail = User.FindFirstValue(ClaimTypes.Email);
            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new { id, name, mail, role });
        }
    }
}

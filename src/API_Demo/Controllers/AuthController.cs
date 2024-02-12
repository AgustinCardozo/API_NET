using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;

        public AuthController(ILogger<AuthController> logger, ILogginService logginService, IConfiguration configuration)
        {
            this.logger = logger;
            this.logginService = logginService;
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("iniciar-seccion")]
        public IActionResult Login(UsuarioLogginReq user)
        {
            try
            {
                logger.LogInformation("Generando token de inicio de seccion");
                return Ok(logginService.IniciarSeccion(user));
            }
            catch (Exception ex)
            {
                logger.LogError(ErrorMessage.GetException(ex));
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
                return Ok(logginService.RegistrarUsuario(user));
            }
            catch (Exception ex)
            {
                logger.LogError(ErrorMessage.GetException(ex));
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("usuario"), Authorize(Roles = Consts.ADMIN)]
        public IActionResult GetUsuario()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = User.FindFirstValue(ClaimTypes.Name);
            var mail = User.FindFirstValue(ClaimTypes.Email);
            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new { id, name, mail, role });
        }

        [HttpGet("password/{pass}"), Authorize(Roles = Consts.ADMIN)]
        public IActionResult GetPassword(string pass)
        {
            return Ok(MD5Service.Decrypt(pass, configuration.GetValue<string>("Hash")));
        }

        [HttpPost("reestablecer-password"), AllowAnonymous]
        public IActionResult ModificarPasswordUsuario(ReestrablecerPassReq user)
        {
            try
            {
                logginService.RestaurarPassword(user.username, user.password);
                return Ok();
            }
            catch(Exception ex)
            {
                return Problem(ErrorMessage.GetException(ex));
            }
        }
    }
}

using API_Demo.Models.Requests;
using API_Demo.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace API_Demo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IJwtTokenService jwtTokenService;

        public AuthController(ILogger<AuthController> logger, IJwtTokenService jwtTokenService)
        {
            this.logger = logger;
            this.jwtTokenService = jwtTokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        public IActionResult Login(UsuarioLoggin user)
        {
            try
            {
                logger.LogInformation("Generando token");
                string token = string.Empty;
                token = jwtTokenService.Authenticate(user.username, user.password);
                return Ok(token);
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error: {ex.Message} {(ex.InnerException != null ? $" - InnerException: " + ex.InnerException.Message : "")} - StackTrace: {ex.StackTrace}";
                logger.LogError(exceptionMessage);
                return BadRequest(ex.Message);
            }
        }
    }
}

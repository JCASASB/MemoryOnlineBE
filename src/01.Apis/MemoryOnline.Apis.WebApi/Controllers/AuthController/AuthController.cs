using MediatR;
using MemoryOnline.Apis.WebApi.Controllers.AuthController.Dtos;
using MemoryOnline.Application.Users.UsersApplication.Commands.Create;
using MemoryOnline.Application.Users.UsersApplication.Queries.GetUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MemoryOnline.Apis.WebApi.Controllers.AuthController
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        private readonly IMediator _mediator;
        public AuthController(IConfiguration config
            , ILogger<AuthController> logger
            , IMediator mediator)
        {
            _config = config;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto login)
        {
            var user = await _mediator.Send(new GetUserQuery(login.UserName));

            if (user != null && user.Password == login.Password)
            {
                var token = GenerarToken(user.Name, user.Id.ToString());
                return Ok(new { token });
            }
            //si no existe, de momento lo creamos para que funcione en desarrollo
            else {
                var newUser = await _mediator.Send(new CreateUserCommand(login.UserName, login.Password));

                    var token = GenerarToken(newUser.Name, newUser.Id.ToString());
                    return Ok(new { token });
                
            }

            return Unauthorized("Usuario o contraseña incorrectos");
        }

        private string GenerarToken(string username, string userId)
        {
            var jwtKey = _config["JwtSettings:Key"];

            // Validación básica para evitar errores al arrancar
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("La JWT SecretKey no está configurada.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Datos que viajan dentro del token (Claims)
            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(ClaimTypes.Role, "Jugador"),
            new Claim(username, "NameUsuario")
        };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60), // Expira en 1 hora
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

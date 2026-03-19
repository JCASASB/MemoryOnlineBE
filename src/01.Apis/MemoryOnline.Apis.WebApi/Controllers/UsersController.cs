using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MemoryOnline.Apis.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            // Ejemplo: lógica para obtener usuario (mock)
            var user = new { Id = id, Name = "Usuario de ejemplo" };
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] dynamic userDto)
        {
            // Ejemplo: lógica para crear usuario (mock)
            var createdUser = new { Id = Guid.NewGuid(), Name = userDto?.Name ?? "SinNombre" };
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }
    }
}

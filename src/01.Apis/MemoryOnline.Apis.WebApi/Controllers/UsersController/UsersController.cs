using MediatR;
using MemoryOnline.Application.Application.UsersApplication.Commands.Create;
using MemoryOnline.Application.Application.UsersApplication.Queries.GetAllUsers;
using MemoryOnline.Application.Application.UsersApplication.Queries.GetUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryOnline.Apis.WebApi.Controllers.UsersController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;

        public UsersController(ILogger<UsersController> logger
            , IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
           
            return Ok();
        }

        // GET: api/users/getbyname/{name}

        [HttpGet("/getbyname/{name}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var users = _mediator.Send(new GetUserQuery(name)).Result;
            return Ok();
        }

        // GET: api/users/getall
        [Authorize]
        [HttpGet("/getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _mediator.Send(new GetAllUsersQuery()).Result;

            return Ok(users);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] dynamic userDto)
        {
            var user = _mediator.Send(new CreateUserCommand(userDto));

            return Ok(user);
        }
    }
}

using MediatR;
using MemoryOnline.Application.Users.UsersApplication.Commands.Create;
using MemoryOnline.Application.Users.UsersApplication.Queries.GetAllUsers;
using MemoryOnline.Application.Users.UsersApplication.Queries.GetUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryOnline.Apis.WebApi.Controllers.ProfilesController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : BaseController
    {
        private readonly ILogger<ProfilesController> _logger;
        private readonly IMediator _mediator;

        public ProfilesController(
            ILogger<ProfilesController> logger
            , IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/profiles/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _mediator.Send(new GetUserQuery("ejemplo"));
            return Ok(user);
        }

        
    }
}

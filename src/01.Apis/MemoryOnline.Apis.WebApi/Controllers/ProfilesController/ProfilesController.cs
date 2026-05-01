using MediatR;
using MemoryOnline.Application.Users.UsersApplication.Queries.GetUser;
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

        // GET: api/profiles/stats/{id}
        [HttpGet("stats/{id}")]
        public async Task<IActionResult> GetUserStats(Guid id)
        {
            var user = await _mediator.Send(new GetUserStatsQuery(id));
            return Ok(user);
        }

        
    }
}

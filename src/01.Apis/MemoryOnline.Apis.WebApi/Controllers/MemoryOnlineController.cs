using MediatR;
using MemoryOnline.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MemoryOnline.Apis.WebApi.Controllers
{  
    [Route("api/[controller]")]
    [ApiController]
    public class MemoryOnlineController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MemoryOnlineController> _logger;

        public MemoryOnlineController(
            IMediator mediator,
            ILogger<MemoryOnlineController> logger
            )
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("game/{id}")]
        public async Task<IActionResult> GetCurrentState(Guid id)
        {
            try
            {
                var response = await _mediator.Send(new MemoryOnline.Application.Application.Queries.GetGameStateQuery(id.ToString()));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Error");
            }
        }
    }
}

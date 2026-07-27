using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MemoryOnline.Apis.Signalr.Hubs
{
    [Authorize]
    public partial class HubApplication : Hub
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public HubApplication(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            string name = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

            String userId = Context.UserIdentifier ?? "UnknownUser";

            var payload = new
            {
                Id = "",
                PlayerId = userId,
                PlayerName = name,
                Message = "Se ha conectado!",
                SentAtUtc = DateTime.UtcNow,
            };

            await Clients.All.SendAsync("ChatMessageReceived", payload);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            String userId = Context.UserIdentifier ?? "UnknownUser";

            await base.OnDisconnectedAsync(exception);
        }
    }
}

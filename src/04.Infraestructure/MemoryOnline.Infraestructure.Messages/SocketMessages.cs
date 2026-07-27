using MemoryOnline.Apis.Signalr.Hubs;
using MemoryOnline.Infraestructure.IRepository;
using Microsoft.AspNetCore.SignalR;

namespace MemoryOnline.Infraestructure.Messages
{
    public class SocketMessages : IHubDomainEvents
    {
        private readonly IHubContext<HubApplication> _hubContext;

        public SocketMessages(IHubContext<HubApplication> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMessagesUserAsync(List<Guid> playerIds, string message)
        {
           /* await _hubContext.Clients
            .Users(playerIds)
            .SendAsync("RecibirNuevoChallenge", new { Id = message });
           */
            // Implement your logic to send a message to a user via socket
            await Task.CompletedTask;
        }

        public async Task SendMessageToUserAsync(Guid playerId, string message)
        {
            var payload = new
            {
                PlayerId = playerId,
                PlayerName = "ola",
                Message = message,
                SentAtUtc = DateTime.UtcNow,
            };

            await _hubContext.Clients
            .User(playerId.ToString())
            .SendAsync("ChatMessageReceived", payload);

            // Implement your logic to send a message to a user via socket
            await Task.CompletedTask;
        }
    }
}

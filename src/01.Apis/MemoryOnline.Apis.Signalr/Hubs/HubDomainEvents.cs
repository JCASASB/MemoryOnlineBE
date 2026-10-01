using MemoryOnline.Infraestructure.IRepository;
using Microsoft.AspNetCore.SignalR;

namespace MemoryOnline.Apis.Signalr.Hubs
{
    public class HubDomainEvents : IHubDomainEvents
    {
        private readonly IHubContext<HubApplication> _hubContext;

        public HubDomainEvents(IHubContext<HubApplication> hubContext)
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

        public async Task SendMessageToUserAsync(Guid playerId, dynamic payload)
        {
            var reto = new
            {
                PlayerId = playerId,
                PlayerName = "ola",
                Message = "Te han retado",
                SentAtUtc = DateTime.UtcNow,
            };

            await _hubContext.Clients
            .User(playerId.ToString())
            .SendAsync("ChatMessageReceived", reto);

            var challenge = new
            {
                Id = payload.GetType().GetProperty("Id").GetValue(payload, null),
                Player1Id = payload.GetType().GetProperty("Player1Id").GetValue(payload, null),
                Player1Name = payload.GetType().GetProperty("Player1Name").GetValue(payload, null),
                Player2Id = payload.GetType().GetProperty("Player2Id").GetValue(payload, null),
                Player2Name = payload.GetType().GetProperty("Player2Name").GetValue(payload, null),
                MatchId = payload.GetType().GetProperty("MatchId").GetValue(payload, null),
                CreatedAt = payload.GetType().GetProperty("CreatedAt").GetValue(payload, null)
            };

            await _hubContext.Clients
            .User(playerId.ToString())
            .SendAsync("ChallengeReceived", challenge);

            // Implement your logic to send a message to a user via socket
            await Task.CompletedTask;
        }

        public async Task SendMessageChallengeToUserAsync(Guid playerId, dynamic payload)
        {
            var reto = new
            {
                PlayerId = playerId,
                PlayerName = "ola",
                Message = "Te han retado",
                SentAtUtc = DateTime.UtcNow,
            };

            await _hubContext.Clients
            .User(playerId.ToString())
            .SendAsync("ChatMessageReceived", reto);

            var challenge = new
            {
                Id = payload.GetType().GetProperty("Id").GetValue(payload, null),
                Player1Id = payload.GetType().GetProperty("Player1Id").GetValue(payload, null),
                Player1Name = payload.GetType().GetProperty("Player1Name").GetValue(payload, null),
                Player2Id = payload.GetType().GetProperty("Player2Id").GetValue(payload, null),
                Player2Name = payload.GetType().GetProperty("Player2Name").GetValue(payload, null),
                MatchId = payload.GetType().GetProperty("MatchId").GetValue(payload, null),
                CreatedAt = payload.GetType().GetProperty("CreatedAt").GetValue(payload, null)
            };

            await _hubContext.Clients
            .User(playerId.ToString())
            .SendAsync("ChallengeReceived", challenge);

            // Implement your logic to send a message to a user via socket
            await Task.CompletedTask;
        }
    }
}

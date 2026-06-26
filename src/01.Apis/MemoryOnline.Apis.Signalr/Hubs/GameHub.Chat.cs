using MemoryOnline.Domain.Entities.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MemoryOnline.Apis.Signalr.Hubs
{
    [Authorize]
    public partial class GameHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            string name = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

            String userId = Context.UserIdentifier ?? "UnknownUser";

            var payload = new
            {
                PlayerId = userId,
                PlayerName = name,
                Message = "Se ha conectado!",
                SentAtUtc = DateTime.UtcNow,
            };

            await Clients.All.SendAsync("ChatMessageReceived", payload);

            await base.OnConnectedAsync();
        }

        public async Task SendChatMessage(dynamic objectParameter)
        {
            var playerName = objectParameter.GetProperty("playerName").GetString();
            var message = objectParameter.GetProperty("message").GetString();
            var playerId = objectParameter.GetProperty("playerId").GetString();

            var cleanMessage = message?.Trim();
            var cleanPlayerName = playerName?.Trim();

            if (string.IsNullOrWhiteSpace(cleanMessage))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(cleanPlayerName))
            {
                cleanPlayerName = Context.UserIdentifier ?? "UnknownUser";
            }

            var payload = new
            {
                PlayerId = playerId,
                PlayerName = cleanPlayerName,
                Message = cleanMessage,
                SentAtUtc = DateTime.UtcNow,
            };

            await Clients.All.SendAsync("ChatMessageReceived", payload);
        }


       

    }
}

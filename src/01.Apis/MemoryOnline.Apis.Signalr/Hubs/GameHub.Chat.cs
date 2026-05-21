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

            var payload = new
            {
                PlayerName = name,
                Message = "Se ha conectado!!",
                SentAtUtc = DateTime.UtcNow,
            };

            await Clients.All.SendAsync("ChatMessageReceived", payload);

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string playerName, string message)
        {
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
                PlayerName = cleanPlayerName,
                Message = cleanMessage,
                SentAtUtc = DateTime.UtcNow,
            };

            await Clients.All.SendAsync("ChatMessageReceived", payload);
        }

        public Task SendChatMessage(string playerName, string message)
        {
            return SendMessage(playerName, message);
        }

    }
}

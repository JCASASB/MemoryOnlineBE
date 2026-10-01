using Microsoft.AspNetCore.SignalR;

namespace MemoryOnline.Apis.Signalr.Hubs
{
    public partial class HubApplication
    {
        public async Task SendChatMessage(System.Text.Json.JsonElement objectParameter)
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
                Id= Guid.NewGuid(),
                PlayerId = playerId,
                PlayerName = cleanPlayerName,
                Message = cleanMessage,
                SentAtUtc = DateTime.UtcNow,
            };

            await Clients.All.SendAsync("ChatMessageReceived", payload);
        }
    }
}
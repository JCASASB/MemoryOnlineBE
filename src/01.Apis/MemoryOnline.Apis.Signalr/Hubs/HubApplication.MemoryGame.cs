using MemoryOnline.Apis.Utils.DTOs.In;
using MemoryOnline.Apis.Utils.DTOs.Out;
using MemoryOnline.Application.Game.GameAppplication.Commands.CreateChallenge;
using MemoryOnline.Application.Game.GameAppplication.Commands.CreateMatch;
using MemoryOnline.Application.Game.GameAppplication.Commands.UpdateGameState;
using MemoryOnline.Application.Game.GameAppplication.Queries.GetBoardStatesFromVersion;
using MemoryOnline.Application.Game.GameAppplication.Queries.GetMatchIdFromName;
using MemoryOnline.Domain.Entities.Game;
using Microsoft.AspNetCore.SignalR;

namespace MemoryOnline.Apis.Signalr.Hubs
{
    public partial class HubApplication
    {
        private async Task ResponseGameState(string clientGroupId, List<BoardState> newGame)
        {
            var dtoGameState = _mapper.Map<GameStateDtoOut>(newGame);
            var json = System.Text.Json.JsonSerializer.Serialize(dtoGameState);
            var groupClients = Clients.Group(clientGroupId);
            await groupClients.SendAsync("UpdateStatesFromServer", newGame);
            await groupClients.SendAsync("LogFromServer", json);
        }

        public async Task JoinGame(string matchId)
        {
            string clientGroupId = new Guid(matchId).ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, clientGroupId);
        }

        public async Task CreateChallenge(System.Text.Json.JsonElement objectParameter)
        {
            var matchId = objectParameter.GetProperty("matchId").GetString();
            var player1Id = objectParameter.GetProperty("player1Id").GetString();
            var player2Id = objectParameter.GetProperty("player2Id").GetString();

            await _mediator.Send(new CreateChallengeCommand(new Guid(matchId), new Guid(player1Id), new Guid(player2Id)));
        }

        public async Task CreateGame(GameStateDtoIn updatedGame)
        {
            var boardState = _mapper.Map<BoardState>(updatedGame);
            await _mediator.Send(new CreateMatchCommand(boardState, Guid.Parse(updatedGame.Id), updatedGame.Name, updatedGame.Level));
            await Clients.Caller.SendAsync("LogFromServer", "Match creado");
        }

        public async Task UpdateGameState(GameStateDtoIn updatedGame, string matchId)
        {
            var theMatchId = new Guid(matchId);
            var domObj = _mapper.Map<BoardState>(updatedGame);
            await _mediator.Send(new AddNewStateCommand(domObj, theMatchId));
            await ResponseGameState(matchId, new List<BoardState> { domObj });
        }

        public async Task<string> GetMatchIdFromName(string gameName)
        {
            var id = await _mediator.Send(new GetMatchIdFromNameQuery(gameName));
            return id.ToString();
        }

        public async Task<List<BoardState>> GetStatesFromVersion(string matchId, int version)
        {
            var theMatchId = new Guid(matchId);
            var boardStates = await _mediator.Send(new GetBoardStatesFromVersionQuery(theMatchId, version));
            return boardStates;
        }
    }
}
using MapsterMapper;
using MediatR;
using MemoryOnline.Apis.Utils.DTOs.In;
using MemoryOnline.Apis.Utils.DTOs.Out;
using MemoryOnline.Application.Application.Commands.JoinGame;
using MemoryOnline.Application.Application.Commands.UpdateGameState;
using MemoryOnline.Application.Application.Queries;
using MemoryOnline.Domain.Entities.Game;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace MemoryOnline.Apis.Signalr.Hubs
{
    public class GameHub : Hub
    {
        private readonly IMediator _mediator;
            private readonly IMapper _mapper;

        public GameHub(
            IMediator mediator
            , IMapper mapper
            )
        {
            _mediator = mediator;
            _mapper = mapper;
        }
      
        private async Task ResponseGameState(string clientGroupId, GameState newGame)
        {
            var dtoGameState = _mapper.Map<GameStateDtoOut>(newGame);

            var json = JsonConvert.SerializeObject(dtoGameState);
            await Clients.Group(clientGroupId).SendAsync("GameStateUpdated", json);

            await Clients.Group(clientGroupId).SendAsync("LogFromServer", json);
        }

        public async Task JoinGame(string playerName, string gameName)
        {
            Console.WriteLine($"[SignalR] Player {Context.ConnectionId} ({playerName}) joining game {gameName}");

            try
            {
                var newGame = await _mediator.Send(new JoinGameCommand(playerName, gameName));

                string clientGroupId = newGame.Id.ToString();
                await Groups.AddToGroupAsync(Context.ConnectionId, newGame.Id.ToString());

                await ResponseGameState(clientGroupId, newGame);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SignalR] Error joining game {gameName}: {ex.Message}");
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        public async Task LeaveGame(string gameId)
        {
            Console.WriteLine($"[SignalR] Player {Context.ConnectionId} leaving game {gameId}");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
            await Clients.Group(gameId).SendAsync("PlayerLeft", Context.ConnectionId);
            Console.WriteLine($"[SignalR] Player {Context.ConnectionId} successfully left game {gameId}");
        }

        public async Task CreateGame(GameStateDtoIn updatedGame)
        {
            Console.WriteLine($"[SignalR] Player {Context.ConnectionId} ");

            Guid gameId = Guid.NewGuid();

            try
            {
                var domObj = _mapper.Map<GameState>(updatedGame);

                await _mediator.Send(new UpdateGameStateCommand(domObj));

                string clientGroupId = domObj.Id.ToString();

                await Groups.AddToGroupAsync(Context.ConnectionId, clientGroupId);

                await Clients.Group(clientGroupId).SendAsync("LogFromServer", "Se ha creado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SignalR] Error creating game: {ex.Message}");
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }
        public async Task UpdateGameState(GameStateDtoIn updatedGame)
        {
            Console.WriteLine($"[SignalR] UpdateGameState called for game {updatedGame.Id}");

            var domObj = _mapper.Map<GameState>(updatedGame);

            await _mediator.Send(new UpdateGameStateCommand(domObj));

            var clientGroupId = domObj.Id.ToString();
            await ResponseGameState(clientGroupId, domObj);    
        }

        public async Task GetGameState(string gameName)
        {
            Console.WriteLine($"[SignalR] UpdateGameState called for game {gameName}");
            var newGame = await _mediator.Send(new GetGameStateQuery(gameName));
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"[SignalR] Player {Context.ConnectionId} disconnected. Exception: {exception?.Message ?? "None"}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}

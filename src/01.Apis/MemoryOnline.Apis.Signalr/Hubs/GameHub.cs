using MapsterMapper;
using MediatR;
using MemoryOnline.Apis.Utils.DTOs.In;
using MemoryOnline.Apis.Utils.DTOs.Out;
using MemoryOnline.Application.Application.GameAppplication.Commands.CreateMatch;
using MemoryOnline.Application.Application.GameAppplication.Commands.JoinGame;
using MemoryOnline.Application.Application.GameAppplication.Commands.UpdateGameState;
using MemoryOnline.Application.Application.GameAppplication.Queries;
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
      
        private async Task ResponseGameState(string clientGroupId, BoardState newGame)
        {
            var dtoGameState = _mapper.Map<GameStateDtoOut>(newGame);
            var json = JsonConvert.SerializeObject(dtoGameState);

            var groupClients = Clients.Group(clientGroupId);
            await groupClients.SendAsync("GameStateUpdated", json);
            await groupClients.SendAsync("LogFromServer", json);
        }

        public async Task JoinGame(string playerName, string gameName)
        {
            try
            {
                var newGame = await _mediator.Send(new JoinMatchCommand(playerName, gameName));

                var id = await _mediator.Send(new GetMatchIdFromNameQuery(gameName));

                string clientGroupId = id.ToString();
                await Groups.AddToGroupAsync(Context.ConnectionId, clientGroupId);

                var dtoGameState = _mapper.Map<GameStateDtoOut>(newGame);
                var json = JsonConvert.SerializeObject(dtoGameState);

                var groupClients = Clients.Group(clientGroupId);
                await groupClients.SendAsync("SetInitialState", json);
                await groupClients.SendAsync("LogFromServer", json);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        public async Task LeaveGame(string gameId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
            await Clients.Group(gameId).SendAsync("PlayerLeft", Context.ConnectionId);
        }

        public async Task CreateGame(GameStateDtoIn updatedGame)
        {
            try
            {
                Guid newMatchId = new Guid();

                var boardState = _mapper.Map<BoardState>(updatedGame);

                await _mediator.Send(new CreateMatchCommand(boardState, newMatchId));

                var id = await _mediator.Send(new GetMatchIdFromNameQuery(boardState.Name));

                string clientGroupId = id.ToString();
                await Groups.AddToGroupAsync(Context.ConnectionId, clientGroupId);

                await Clients.Caller.SendAsync("LogFromServer", "Match creado");
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }
        public async Task UpdateGameState(GameStateDtoIn updatedGame)
        {
            var domObj = _mapper.Map<BoardState>(updatedGame);

            await _mediator.Send(new AddNewStateCommand(domObj, domObj.Id));

            var id = await _mediator.Send(new GetMatchIdFromNameQuery(domObj.Name));
            string clientGroupId = id.ToString();

            await ResponseGameState(clientGroupId, domObj);    
        }

        public async Task GetGameState(string gameName)
        {
            var newGame = await _mediator.Send(new GetGameStateQuery(gameName));
        }

        public async Task GetMatchIdFromName(string gameName)
        {
            try
            {
                var id = await _mediator.Send(new GetMatchIdFromNameQuery(gameName));
                string clientGroupId = id.ToString();

                await Clients.Caller.SendAsync("SetMatchId", id);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        public async Task GetStatesFromVersion(Guid gameId, int version)
        {
            try
            {
                var boardStates = await _mediator.Send(new GetBoardStatesFromVersionQuery(gameId, version));

                await Clients.Caller.SendAsync("UpdateStatesFromServer", boardStates.ToArray());
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}

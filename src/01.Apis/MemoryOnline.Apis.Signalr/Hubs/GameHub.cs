using MapsterMapper;
using MediatR;
using MemoryOnline.Apis.Utils.DTOs.In;
using MemoryOnline.Apis.Utils.DTOs.Out;
using MemoryOnline.Application.Application.GameAppplication.Commands.CreateMatch;
using MemoryOnline.Application.Application.GameAppplication.Commands.UpdateGameState;
using MemoryOnline.Application.Application.GameAppplication.Queries;
using MemoryOnline.Domain.Entities.Game;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace MemoryOnline.Apis.Signalr.Hubs
{
    [Authorize]
    public partial class GameHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GameHub(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
      
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
            String userId = Context.UserIdentifier ?? "UnknownUser";

            Guid theMatchId = new Guid(matchId);

            string clientGroupId = theMatchId.ToString();

            await Groups.AddToGroupAsync(Context.ConnectionId, clientGroupId);
        }

        public async Task CreateGame(GameStateDtoIn updatedGame)
        {
            String userId = Context.UserIdentifier ?? "UnknownUser";

            Guid newMatchId = Guid.NewGuid();

            var boardState = _mapper.Map<BoardState>(updatedGame);

            await _mediator.Send(new CreateMatchCommand(boardState, newMatchId));

            await Clients.Caller.SendAsync("LogFromServer", "Match creado");
        }

        public async Task UpdateGameState(GameStateDtoIn updatedGame, string matchId)
        {
            String userId = Context.UserIdentifier ?? "UnknownUser";

            var theMatchId = new Guid(matchId);

            var domObj = _mapper.Map<BoardState>(updatedGame);

            await _mediator.Send(new AddNewStateCommand(domObj, theMatchId));

            var id = await _mediator.Send(new GetMatchIdFromNameQuery(domObj.Name));

            string clientGroupId = id.ToString();

            var lista = new List<BoardState>();

            lista.Add(domObj);

            await ResponseGameState(clientGroupId, lista);
        }

        public async Task<string> GetMatchIdFromName(string gameName)
        {
            String userId = Context.UserIdentifier ?? "UnknownUser";

            var id = await _mediator.Send(new GetMatchIdFromNameQuery(gameName));

            return id.ToString();
        }

        public async Task<List<BoardState>> GetStatesFromVersion(string matchId, int version)
        {
            String userId = Context.UserIdentifier ?? "UnknownUser";

            var theMatchId = new Guid(matchId);

            var boardStates = await _mediator.Send(new GetBoardStatesFromVersionQuery(theMatchId, version));

            return boardStates;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            String userId = Context.UserIdentifier ?? "UnknownUser";

            await base.OnDisconnectedAsync(exception);
        }
    }
}

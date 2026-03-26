using MediatR;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Queries
{
    public class GetGameStateHandler : IRequestHandler<GetGameStateQuery, GameState>
    {
        private readonly IGameRepository _gameRepository;

        public GetGameStateHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public Task<GameState> Handle(GetGameStateQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_gameRepository.GetGameByName(request.gameName));
        }
    }
}

using MediatR;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
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

        public async Task<GameState> Handle(GetGameStateQuery request, CancellationToken cancellationToken)
        {
            var games = await _gameRepository.GetAllAsync();
            return games.Where(g => g.Name == request.gameName).First();
        }
    }
}

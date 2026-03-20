using MediatR;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Repository.IRepository;

namespace MemoryOnline.Application.Application.Queries
{
    public class GetGameStateHandler : IRequestHandler<GetGameStateQuery, GameState>
    {
        private readonly IRepositoryGame _gameRepository;

        public GetGameStateHandler(IRepositoryGame gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public Task<GameState> Handle(GetGameStateQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_gameRepository.GetGameByName(request.gameName));
        }
    }
}

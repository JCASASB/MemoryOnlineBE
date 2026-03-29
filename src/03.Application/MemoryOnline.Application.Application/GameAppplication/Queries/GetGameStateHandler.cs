using MediatR;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Queries
{
    public class GetGameStateHandler : IRequestHandler<GetGameStateQuery, GameState>
    {
        private readonly IGenericRepository<GameState> _gameRepository;

        public GetGameStateHandler(IGenericRepository<GameState> gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public Task<GameState> Handle(GetGameStateQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_gameRepository.GetAll().Where(g => g.Name == request.gameName).ToList().First());
        }
    }
}

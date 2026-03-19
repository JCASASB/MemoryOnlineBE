using MediatR;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Repository.IRepository;
using System.Threading;
using System.Threading.Tasks;

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

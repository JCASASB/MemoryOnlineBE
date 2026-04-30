using MediatR;
using MemoryOnline.Infraestructure.IRepository.Game;

namespace MemoryOnline.Application.Application.GameAppplication.Queries
{
    public class GetMatchIdFromNameHandler : IRequestHandler<GetMatchIdFromNameQuery, Guid>
    {
        private readonly IGameRepository _gameRepository;

        public GetMatchIdFromNameHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Guid> Handle(GetMatchIdFromNameQuery request, CancellationToken cancellationToken)
        {
            var match = await _gameRepository.GetMatchByNameAsync(request.gameName);

            return match.Id;
        }
    }
}

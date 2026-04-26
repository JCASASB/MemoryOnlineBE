using MediatR;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Queries
{
    public class GetMatchIdFromNameHandler : IRequestHandler<GetMatchIdFromNameQuery, Guid>
    {
        private readonly IMatchRepository _gameRepository;

        public GetMatchIdFromNameHandler(IMatchRepository gameRepository)
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

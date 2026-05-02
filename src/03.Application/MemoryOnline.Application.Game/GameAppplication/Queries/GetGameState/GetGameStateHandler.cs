using MediatR;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository.Game;

namespace MemoryOnline.Application.Game.GameAppplication.Queries.GetGameState
{
    public class GetGameStateHandler : IRequestHandler<GetGameStateQuery, BoardState>
    {
        private readonly IGameRepository _gameRepository;

        public GetGameStateHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<BoardState> Handle(GetGameStateQuery request, CancellationToken cancellationToken)
        {
           throw new NotImplementedException();
        }
    }
}

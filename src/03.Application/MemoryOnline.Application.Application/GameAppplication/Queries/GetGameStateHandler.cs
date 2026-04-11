using MediatR;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Queries
{
    public class GetGameStateHandler : IRequestHandler<GetGameStateQuery, BoardState>
    {
        private readonly IMatchRepository _gameRepository;

        public GetGameStateHandler(IMatchRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<BoardState> Handle(GetGameStateQuery request, CancellationToken cancellationToken)
        {
           throw new NotImplementedException();
        }
    }
}

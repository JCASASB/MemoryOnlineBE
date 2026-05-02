using MediatR;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository.Game;

namespace MemoryOnline.Application.Game.GameAppplication.Queries.GetAllBoardStates
{
    public class GetAllBoardStatesHandler : IRequestHandler<GetAllBoardStatesQuery, List<BoardState>>
    {
        private readonly IGameRepository _matchRepository;

        public GetAllBoardStatesHandler(IGameRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<List<BoardState>> Handle(GetAllBoardStatesQuery request, CancellationToken cancellationToken)
        {
            var allStates = await _matchRepository.GetAllBoardStatesAsync(request.mathId);

            return allStates.ToList();
        }
    }
}

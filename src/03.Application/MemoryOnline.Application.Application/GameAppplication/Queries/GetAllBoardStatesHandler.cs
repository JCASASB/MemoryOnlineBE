using MediatR;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Queries
{
    public class GetAllBoardStatesHandler : IRequestHandler<GetAllBoardStatesQuery, List<BoardState>>
    {
        private readonly IMatchRepository _matchRepository;

        public GetAllBoardStatesHandler(IMatchRepository matchRepository)
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

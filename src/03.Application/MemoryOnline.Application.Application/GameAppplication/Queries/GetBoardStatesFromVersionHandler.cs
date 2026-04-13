using MediatR;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Queries
{
    public class GetBoardStatesFromVersionHandler : IRequestHandler<GetBoardStatesFromVersionQuery, List<BoardState>>
    {
        private readonly IMatchRepository _matchRepository;

        public GetBoardStatesFromVersionHandler(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<List<BoardState>> Handle(GetBoardStatesFromVersionQuery request, CancellationToken cancellationToken)
        {
            var allStates = await _matchRepository.GetAllBoardStatesAsync(request.mathId);

            return allStates.Where(s=> s.Version >= request.version).ToList();
        }
    }
}

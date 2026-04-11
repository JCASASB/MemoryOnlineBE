using MemoryOnline.Domain.Domain.IMatchUseCases;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.MatchUseCases
{
    public class CreateMatchUseCase : ICreateMatchUseCase
    {
        public Match Execute(BoardState initialState, Guid matchId)
        {
            Match match = new Match();
            match.Id = matchId;
            match.States = new List<BoardState> { initialState };

            return match;
        }
    }
}

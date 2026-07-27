using MemoryOnline.Domain.Domain.IMatchUseCases;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.MatchUseCases
{
    public class CreateMatchUseCase : ICreateMatchUseCase
    {
        public Match Execute(BoardState initialState, Guid idMatch, string name, int level)
        {
            Match match = new Match() { 
                Name = name,
                Level = level,
                States = new() 
            };
            match.Id = idMatch; 
            match.States = new List<BoardState> { initialState };

            return match;
        }
    }
}

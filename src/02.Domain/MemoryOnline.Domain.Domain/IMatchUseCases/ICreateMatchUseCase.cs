using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.IMatchUseCases
{
    public interface ICreateMatchUseCase
    {
        Match Execute(BoardState initialState, Guid idMatch, string name, int level);
    }
}

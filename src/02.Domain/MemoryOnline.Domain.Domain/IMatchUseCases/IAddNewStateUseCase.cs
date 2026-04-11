using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.IGameUseCases
{
    public interface IAddNewStateUseCase
    {
        Match Execute(Match match, BoardState newState);
    }
}

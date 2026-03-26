using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.IGameUseCases
{
    public interface IUpdateStateUseCase
    {
        GameState Execute(GameState game);
    }
}

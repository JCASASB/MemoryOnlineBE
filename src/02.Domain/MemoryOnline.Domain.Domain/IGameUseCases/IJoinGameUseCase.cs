using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.GameUseCases
{
    public interface IJoinGameUseCase
    {
        GameState Execute(GameState game, string playerName);
    }
}

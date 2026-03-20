using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.IGameUseCases
{
    public interface IClickCardUseCase
    {
        GameState Execute(GameState game, Guid cardId, Guid playerId);
    }
}
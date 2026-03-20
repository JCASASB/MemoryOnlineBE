
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.GameUseCases
{
    public class ClickCardUseCase : IClickCardUseCase
    {
        public GameState Execute(GameState game, Guid cardId, Guid playerId)
        {
            return game;
        }
    }
}

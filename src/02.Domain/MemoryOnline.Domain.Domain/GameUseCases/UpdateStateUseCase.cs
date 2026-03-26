
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.GameUseCases
{
    public class UpdateStateUseCase : IUpdateStateUseCase
    {
        public GameState Execute(GameState game)
        {
            return game;
        }
    }
}

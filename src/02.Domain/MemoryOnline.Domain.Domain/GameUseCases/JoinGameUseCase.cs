
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.GameUseCases
{
    public class JoinGameUseCase : IJoinGameUseCase
    {
        public GameState Execute(GameState game, string playerName)
        {
            var player = new Player.Builder()
                .WithId(Guid.NewGuid())
                .WithName(playerName)
                .WithTurn(true)
                // .WithPoints(0) -> Esto debería ser el default en el Builder
                .Build();

            game.AddPlayer(player);

            // NO reinicializar cartas cuando alguien se une - solo inicializar en la primera llamada
            // o cuando se crea el juego
            game.InitializeCards(reinitialize: false);

            return game;
        }

    }
}

using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Domain.Domain.UseCases
{
    public class CreateGameUseCase
    {
        public GameState Execute(string playerName, string gameName, Guid gameId, int level)
        {
            // El Builder de Player debería ocultar detalles técnicos como Moves = 0
            var player = new Player.Builder()
                .WithId(Guid.NewGuid())
                .WithName(playerName)
                // .WithPoints(0) -> Esto debería ser el default en el Builder
                .Build();

            var game = new GameState.Builder()
                .WithId(gameId)
                .WithName(gameName)
                .WithLevel(level)
                // .WithIsProcessing(false) -> Default en el Builder
                .Build();

            // 1. Asociamos el jugador (EF detectará la relación automáticamente)
            game.AddPlayer(player);

            return game;
        }

    }
}

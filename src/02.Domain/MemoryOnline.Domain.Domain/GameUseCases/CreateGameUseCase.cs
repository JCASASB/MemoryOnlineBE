using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.GameUseCases
{
    public class CreateGameUseCase : ICreateGameUseCase
    {
        public GameState Execute(string playerName, string gameName, Guid gameId, int level)
        {
            // Crear el juego como POCO
            var game = new GameState
            {
                Id = gameId,
                Name = gameName,
                Level = level,
                Version = 0,
                Players = new List<Player>(),
                Cards = new List<Card>()
            };

            // Crear el primer jugador con Id
            var player = new Player
            {
                Id = Guid.NewGuid(),
                Name = playerName,
                Turn = true // El primer jugador empieza
            };

            // Asociar el jugador al juego
            game.Players.Add(player);

            return game;
        }
    }
}

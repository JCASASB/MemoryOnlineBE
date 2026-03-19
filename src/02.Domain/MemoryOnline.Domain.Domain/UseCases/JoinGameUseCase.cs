using System;
using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Domain.Domain.UseCases
{
    public class JoinGameUseCase
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

            game.InitializeCards();

            return game;
        }
    }
}

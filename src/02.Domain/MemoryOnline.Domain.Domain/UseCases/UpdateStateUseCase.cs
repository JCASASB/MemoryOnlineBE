using System;
using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Domain.Domain.UseCases
{
    public class UpdateStateUseCase
    {
        public GameState Execute(GameState game)
        {
            return game;
        }
    }
}

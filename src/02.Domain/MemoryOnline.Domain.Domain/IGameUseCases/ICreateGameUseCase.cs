using MemoryOnline.Domain.Entities.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Domain.Domain.IGameUseCases
{
    public interface ICreateGameUseCase
    {
        GameState Execute(string playerName, string gameName, Guid gameId, int level);
    }
}

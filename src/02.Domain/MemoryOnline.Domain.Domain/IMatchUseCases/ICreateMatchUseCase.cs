using MemoryOnline.Domain.Entities.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Domain.Domain.IMatchUseCases
{
    public interface ICreateMatchUseCase
    {
        Match Execute(BoardState initialState, Guid matchId);
    }
}

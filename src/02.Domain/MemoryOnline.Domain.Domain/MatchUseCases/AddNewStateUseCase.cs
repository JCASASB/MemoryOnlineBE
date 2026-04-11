
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.GameUseCases
{
    public class AddNewStateUseCase : IAddNewStateUseCase
    {
        public Match Execute(Match match, BoardState newState)
        {
            match.States.Add(newState);

            return match;
        }
    }
}

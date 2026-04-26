using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.IGameUseCases
{
    public interface IAddNewStateUseCase : IEventDomainBase
    {
        Match Execute(Match match, BoardState newState);
    }
}

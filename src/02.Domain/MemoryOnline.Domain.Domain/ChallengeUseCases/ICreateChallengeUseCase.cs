using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.ChallengeUseCases
{
    public interface ICreateChallengeUseCase : IEventDomainBase
    {
        Challenge Execute(Match match, Player p1, Player p2);
    }
}

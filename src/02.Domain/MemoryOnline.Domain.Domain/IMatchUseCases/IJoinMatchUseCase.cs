using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.MatchUseCases
{
    public interface IJoinMatchUseCase
    {
        BoardState Execute(Match match, string playerName);
    }
}

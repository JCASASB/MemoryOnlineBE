using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Game.Events;
using Match = MemoryOnline.Domain.Entities.Game.Match;

namespace MemoryOnline.Domain.Domain.ChallengeUseCases
{
    public class CreateChallengeUseCase : DomainUseCaseBase, ICreateChallengeUseCase
    {
        public Challenge Execute(Match match, Player p1, Player p2)
        {
            Challenge challenge = new Challenge()
            {
                Id = Guid.NewGuid(),
                Match = match,
                Players = new List<Player>() { p1, p2 }
            };

            this.AddDomainEvent(new ChallengeCreatedDomainEvent(challenge.Id));

            return challenge;
        }
    }
}
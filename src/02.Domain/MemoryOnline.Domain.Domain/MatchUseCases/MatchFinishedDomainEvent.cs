using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Domain.Domain.MatchUseCases
{
    public class MatchFinishedDomainEvent : DomainEvent
    {
        public Guid MatchId { get; }

        public MatchFinishedDomainEvent(Guid matchId) 
        {
            MatchId = matchId;
        }
    }
}

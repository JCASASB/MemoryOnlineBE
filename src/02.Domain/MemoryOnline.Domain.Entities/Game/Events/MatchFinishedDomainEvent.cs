namespace MemoryOnline.Domain.Entities.Game.Events
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

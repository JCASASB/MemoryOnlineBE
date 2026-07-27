namespace MemoryOnline.Domain.Entities.Game.Events
{
    public class ChallengeCreatedDomainEvent : DomainEvent
    {
        public Guid ChallengeId { get; }

        public ChallengeCreatedDomainEvent(Guid challengeId) 
        {
            ChallengeId = challengeId;
        }
    }
}

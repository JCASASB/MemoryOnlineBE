using MemoryOnline.Domain.Entities;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.MatchUseCases
{
    public class MatchFinishedDomainEvent : DomainEvent
    {
        public Guid MatchId { get; }
        public Guid WinnerId { get; }

        public MatchFinishedDomainEvent(Guid matchId, BoardState newState) 
        {
            MatchId = matchId;
            var winner = newState.Players.OrderByDescending(p => p.Points).FirstOrDefault();
            WinnerId = winner != null ? winner.Id : Guid.Empty;
        }
    }
}

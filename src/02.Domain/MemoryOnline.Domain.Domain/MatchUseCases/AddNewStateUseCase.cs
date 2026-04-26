
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Domain.Domain.MatchUseCases;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Domain.Domain.GameUseCases
{
    public class AddNewStateUseCase : DomainUseCaseBase ,IAddNewStateUseCase
    {
        public Match Execute(Match match, BoardState newState)
        {
            match.States.Add(newState);

            if (AreAllCardsMatched(newState))
            {
                this.AddDomainEvent(new MatchFinishedDomainEvent(match.Id, newState));
            }

            return match;
        }

        // Regla de negocio: Si están todas emparejadas, el juego terminó
        private bool AreAllCardsMatched(BoardState newState)
        {
            if (newState == null || newState.Cards == null || newState.Cards.Count == 0)
                return false;

            return newState.Cards.All(c => c.State == EnumCardState.Matched);
        }
    }
}

using MediatR;
using MemoryOnline.Application.Application.Events;
using MemoryOnline.Domain.Domain.MatchUseCases;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Infraestructure.IRepository.Application;
using MemoryOnline.Infraestructure.IRepository.Game;

namespace MemoryOnline.Application.Game.GameAppplication.EventHandlers
{
    public class MatchFinishedEventHandler : INotificationHandler<DomainEventNotificationAdaptor<MatchFinishedDomainEvent>>
    {
        
        private readonly IApplicationUOW _appUOW;
        private readonly IGameRepository _gameRepository;

        public MatchFinishedEventHandler(IApplicationUOW appUOW, IGameRepository gameRepository)
        {
             _appUOW = appUOW;
             _gameRepository = gameRepository;
        }

        public async Task Handle(DomainEventNotificationAdaptor<MatchFinishedDomainEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            
            var matchId = domainEvent.MatchId;

            var allStates = await _gameRepository.GetAllBoardStatesAsync(matchId);
            var lastState = allStates.OrderByDescending(s => s.Version).FirstOrDefault();

            if (lastState != null)
            {
                foreach (var player in lastState.Players) {

                    UserMatchResult resultsPlayer = new UserMatchResult();
                    resultsPlayer.MatchId = matchId;
                    resultsPlayer.Id = Guid.NewGuid();
                    resultsPlayer.Moves = player.TotalMoves;
                    resultsPlayer.Fails = player.TotalMoves - player.Points;
                    resultsPlayer.Matchs = player.Points;
                    resultsPlayer.UsuarioId = player.Id;

                    await _appUOW.AddUserResultsAsync(resultsPlayer);
                }

            }

            // Remueve esta línea cuando tengas tus llamadas asíncronas reales a la base de datos
            await Task.CompletedTask; 
        }
    }
}

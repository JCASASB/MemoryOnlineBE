using MediatR;
using MemoryOnline.Application.Application.Events;
using MemoryOnline.Domain.Domain.MatchUseCases;
using MemoryOnline.Domain.Entities.Stats;
using MemoryOnline.Infraestructure.IRepository.Application;

namespace MemoryOnline.Application.Users.UsersApplication.EventHandlers
{
    public class MatchFinishedEventHandler : INotificationHandler<DomainEventNotificationAdaptor<MatchFinishedDomainEvent>>
    {
        
         private readonly IApplicationRepository _appRepository;

        public MatchFinishedEventHandler(IApplicationRepository appRepository)
        {
             _appRepository = appRepository;
        }

        public async Task Handle(DomainEventNotificationAdaptor<MatchFinishedDomainEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            
            var matchId = domainEvent.MatchId;
            var winnerId = domainEvent.WinnerId;

            if (winnerId != System.Guid.Empty)
            {
                UserResults results = new UserResults();
                results.MatchId = matchId;
                results.Id = matchId;
                results.Moves = 1;
                results.Fails = 0;
                results.Matchs = 0;
                results.Winner = false;

                await _appRepository.AddUserResultsAsync(results);
            }

            // Remueve esta línea cuando tengas tus llamadas asíncronas reales a la base de datos
            await Task.CompletedTask; 
        }
    }
}

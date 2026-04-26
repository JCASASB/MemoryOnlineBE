using MediatR;
using MemoryOnline.Application.Application.Events;
using MemoryOnline.Domain.Domain.MatchUseCases;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Users.UsersApplication.EventHandlers
{
    public class MatchFinishedEventHandler : INotificationHandler<DomainEventNotificationAdaptor<MatchFinishedDomainEvent>>
    {
        
         private readonly IUsersRepository _userRepository;

        public MatchFinishedEventHandler(IUsersRepository userRepository)
        {
             _userRepository = userRepository;
        }

        public async Task Handle(DomainEventNotificationAdaptor<MatchFinishedDomainEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            
            var matchId = domainEvent.MatchId;
            var winnerId = domainEvent.WinnerId;

            if (winnerId != System.Guid.Empty)
            {
              //  _userRepository
            }

            // Remueve esta línea cuando tengas tus llamadas asíncronas reales a la base de datos
            await Task.CompletedTask; 
        }
    }
}

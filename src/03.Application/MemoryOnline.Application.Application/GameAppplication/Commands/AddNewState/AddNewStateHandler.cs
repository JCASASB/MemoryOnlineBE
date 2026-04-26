using MediatR;
using MemoryOnline.Application.Application.Events;
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Commands.UpdateGameState
{
    public class AddNewStateHandler : IRequestHandler<AddNewStateCommand>
    {
        private readonly IMatchRepository _gameRepository;
        private readonly IAddNewStateUseCase _addNewStateUseCase;
        private readonly IMediator _mediator;

        public AddNewStateHandler(
            IMatchRepository gameRepository
            , IAddNewStateUseCase addNewStateUseCase
            , IMediator mediator)
        {
            _gameRepository = gameRepository;
            _addNewStateUseCase = addNewStateUseCase;
            _mediator = mediator;
        }

        public async Task Handle(AddNewStateCommand request, CancellationToken cancellationToken)
        {
            var match = await _gameRepository.GetMatchByNameAsync(request.gameState.Name);
            
            match = _addNewStateUseCase.Execute(match, request.gameState);

            await _gameRepository.UpdateAsync(match);

            await ThrowTheNewDomainEvents(_addNewStateUseCase.GetEvents(), cancellationToken);
        }

        private async Task ThrowTheNewDomainEvents(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
        {
            foreach (var domainEvent in events)
            {
                var eventWrapperType = typeof(DomainEventNotificationAdaptor<>).MakeGenericType(domainEvent.GetType());
                var eventWrapper = (INotification)Activator.CreateInstance(eventWrapperType, domainEvent);

                if (eventWrapper != null)
                {
                    await _mediator.Publish(eventWrapper, cancellationToken);
                }
            }   
        }
    }
}

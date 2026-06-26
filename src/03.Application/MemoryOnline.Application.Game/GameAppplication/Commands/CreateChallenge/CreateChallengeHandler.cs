using MediatR;
using MemoryOnline.Application.Application.Events;
using MemoryOnline.Domain.Domain.ChallengeUseCases;
using MemoryOnline.Domain.Domain.Specifications.Implementations;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Users;
using MemoryOnline.Infraestructure.IRepository.Application;
using MemoryOnline.Infraestructure.IRepository.Game;

namespace MemoryOnline.Application.Game.GameAppplication.Commands.CreateChallenge
{
     public class CreateChallengeHandler : IRequestHandler<CreateChallengeCommand>
    {
        private readonly IGameRepository _gameRepository;
        private readonly IApplicationUOW _userRepository;
        private readonly ICreateChallengeUseCase _createChallengeUseCase;
        private readonly IMediator _mediator;

        public CreateChallengeHandler(
            IGameRepository gameRepository
            , IApplicationUOW userRepository
            , ICreateChallengeUseCase createChallengeUseCase
            , IMediator mediator)
        {
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _createChallengeUseCase = createChallengeUseCase;
            _mediator = mediator;
        }

        public async Task Handle(CreateChallengeCommand request, CancellationToken cancellationToken)
        {
            var match = await _gameRepository.GetMatchByIdAsync(request.matchId);

            if (match == null)
                throw new Exception($"Match con Id {request.matchId} no encontrado.");

            Usuario u1 = await this.GetTheUserPlayer(request.playerId1);
            Usuario u2 = await this.GetTheUserPlayer(request.playerId2);

            Player p1 = new Player()
            {
                Id = u1.Id,
                Name = u1.Name,
            };

            Player p2 = new Player()
            {
                Id = u2.Id,
                Name = u2.Name,
            };
            
            Challenge challenge = _createChallengeUseCase.Execute(match, p1, p2);

            await _gameRepository.AddChallengeAsync(challenge);

            await ThrowTheNewDomainEvents(_createChallengeUseCase.GetEvents(), cancellationToken);
        }

        public async Task<Usuario> GetTheUserPlayer(Guid idPlayer)
        {
            var filterSpec = new UserFilterByIdSpec(idPlayer);

            var users = await _userRepository.GetUserWithFilter(filterSpec);   
            
            return users.First();
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

using MediatR;
using MemoryOnline.Application.Application.Events;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Domain.Entities.Game.Events;
using MemoryOnline.Infraestructure.IRepository;
using MemoryOnline.Infraestructure.IRepository.Game;
using System.Numerics;
using System.Text.RegularExpressions;

namespace MemoryOnline.Application.Game.GameAppplication.EventHandlers
{
    public class ChallengeCreatedEventHandler : INotificationHandler<DomainEventNotificationAdaptor<ChallengeCreatedDomainEvent>>
    {
        private readonly ISocketMessages _socketMessages;
        private readonly IGameRepository _gameRepository;
        public ChallengeCreatedEventHandler(ISocketMessages socketMessages, IGameRepository gameRepository)
        {
            _socketMessages = socketMessages;
            _gameRepository = gameRepository;
        }

        public async Task Handle(DomainEventNotificationAdaptor<ChallengeCreatedDomainEvent> notification, CancellationToken cancellationToken)
        {
            Challenge challenge = await _gameRepository.GetChallengeAsync(notification.DomainEvent.ChallengeId);

            dynamic payload = new
            {
                Id = challenge.Id.ToString(),
                Player1Id = challenge.Players[0].Id.ToString(),
                Player1Name = challenge.Players[0].Name,
                Player2Id = challenge.Players[1].Id.ToString(),
                Player2Name = challenge.Players[1].Name,
                MatchId = challenge.Match.Id.ToString(),
            };

            if (challenge != null) {
                foreach (var player in challenge.Players)
                {
                    // Aquí puedes personalizar el mensaje que deseas enviar a cada jugador
                    string message = $"¡Hola {player.Name}! Se ha creado un nuevo desafío en el juego. ¡Prepárate para jugar!";
                   
                    await _socketMessages.SendMessageToUserAsync(player.Id, payload);
                }

            // Remueve esta línea cuando tengas tus llamadas asíncronas reales a la base de datos
            await Task.CompletedTask;
        }
    }
}
}

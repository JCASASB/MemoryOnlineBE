using MediatR;
using MemoryOnline.Application.Game.GameAppplication.Commands.CreateMatch;
using MemoryOnline.Domain.Domain.IMatchUseCases;
using MemoryOnline.Infraestructure.IRepository.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Application.Game.GameAppplication.Commands.CreateChallenge
{
     public class CreateChallengeHandler : IRequestHandler<CreateChallengeCommand>
    {
        private readonly IGameRepository _gameRepository;

        public CreateChallengeHandler(
            IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task Handle(CreateChallengeCommand request, CancellationToken cancellationToken)
        {
            await _gameRepository.AddMatchAsync(match);
        }
    }
}

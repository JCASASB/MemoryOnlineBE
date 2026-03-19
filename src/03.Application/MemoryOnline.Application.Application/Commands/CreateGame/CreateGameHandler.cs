using MediatR;
using MemoryOnline.Domain.Domain.UseCases;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Repository.IRepository;   

namespace MemoryOnline.Application.Application.Commands.CreateGame
{
    public class CreateGameHandler : IRequestHandler<CreateGameCommand, GameState>
    {
        private readonly CreateGameUseCase _createGameUseCase = new();
        private readonly IRepositoryGame _gameRepository;

        public CreateGameHandler(IRepositoryGame gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public Task<GameState> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var game = _createGameUseCase.Execute(request.PlayerName, request.GameName, request.GameId, request.Level);
            _gameRepository.AddGame(game);
            return Task.FromResult(game);
        }
    }
}

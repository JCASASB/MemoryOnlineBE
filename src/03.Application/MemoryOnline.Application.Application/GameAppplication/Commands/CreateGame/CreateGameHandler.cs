using MediatR;
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Commands.CreateGame
{
    public class CreateGameHandler : IRequestHandler<CreateGameCommand>
    {
        private readonly ICreateGameUseCase _createGameUseCase;
        private readonly IGenericRepository<GameState> _gameRepository;

        public CreateGameHandler(
            IGenericRepository<GameState> gameRepository
            , ICreateGameUseCase createGameUseCase)
        {
            _createGameUseCase = createGameUseCase;
            _gameRepository = gameRepository;
        }

        public async Task Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var game = _createGameUseCase.Execute(request.PlayerName, request.GameName, request.GameId, request.Level);
            _gameRepository.Add(game);
            _gameRepository.SaveChanges();

            await Task.CompletedTask;
        }
    }
}

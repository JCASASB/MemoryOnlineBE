using MediatR;
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Commands.CreateGame
{
    public class CreateGameHandler : IRequestHandler<CreateGameCommand>
    {
        private readonly ICreateGameUseCase _createGameUseCase;
        private readonly IGameRepository _gameRepository;

        public CreateGameHandler(
            IGameRepository gameRepository
            , ICreateGameUseCase createGameUseCase)
        {
            _createGameUseCase = createGameUseCase;
            _gameRepository = gameRepository;
        }

        public async Task Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var game = _createGameUseCase.Execute(request.PlayerName, request.GameName, request.GameId, request.Level);
            _gameRepository.AddGame(game);
         
            await Task.CompletedTask;
        }
    }
}

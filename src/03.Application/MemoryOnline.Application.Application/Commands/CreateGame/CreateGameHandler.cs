using MediatR;
using MemoryOnline.Domain.Domain.GameUseCases;
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Repository.IRepository;   

namespace MemoryOnline.Application.Application.Commands.CreateGame
{
    public class CreateGameHandler : IRequestHandler<CreateGameCommand>
    {
        private readonly ICreateGameUseCase _createGameUseCase;
        private readonly IRepositoryGame _gameRepository;

        public CreateGameHandler(
            IRepositoryGame gameRepository
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

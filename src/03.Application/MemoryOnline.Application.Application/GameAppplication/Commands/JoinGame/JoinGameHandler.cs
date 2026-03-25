using MediatR;
using MemoryOnline.Domain.Domain.GameUseCases;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Commands.JoinGame
{
    public class JoinGameHandler : IRequestHandler<JoinGameCommand, GameState>
    {
        private readonly IJoinGameUseCase _joinGameUseCase;
        private readonly IGameRepository _gameRepository;

        public JoinGameHandler(
            IGameRepository gameRepository
            , IJoinGameUseCase joinGameUseCase)
        {
            _joinGameUseCase = joinGameUseCase;
            _gameRepository = gameRepository;
        }

        public Task<GameState> Handle(JoinGameCommand request, CancellationToken cancellationToken)
        {
            var board = _gameRepository.GetGameByName(request.gameName);

            if (board == null)
                throw new KeyNotFoundException($"Game '{request.gameName}' not found");

            board = _joinGameUseCase.Execute(board, request.playerName);

            _gameRepository.UpdateGame(board);

            return Task.FromResult(board);
        }
    }
}

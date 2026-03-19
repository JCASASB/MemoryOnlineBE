using MediatR;
using MemoryOnline.Domain.Domain.UseCases;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Repository.IRepository;

namespace MemoryOnline.Application.Application.Commands.JoinGame
{
    public class JoinGameHandler : IRequestHandler<JoinGameCommand, GameState>
    {
        private readonly JoinGameUseCase _joinGameUseCase = new();
        private readonly IRepositoryGame _gameRepository;

        public JoinGameHandler(IRepositoryGame gameRepository)
        {
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

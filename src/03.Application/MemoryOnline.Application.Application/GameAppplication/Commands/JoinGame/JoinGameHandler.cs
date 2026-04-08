using MediatR;
using MemoryOnline.Domain.Domain.GameUseCases;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
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

        public async Task<GameState> Handle(JoinGameCommand request, CancellationToken cancellationToken)
        {
            var boards = await _gameRepository.GetGameByNameAsync(request.gameName);
            var board = boards.FirstOrDefault();

            if (board == null)
                throw new KeyNotFoundException($"Game '{request.gameName}' not found");

            Console.WriteLine($"Game found: {board.Name}, Players count: {board.Players?.Count ?? 0}");

            board = _joinGameUseCase.Execute(board, request.playerName);

            await _gameRepository.UpdateAsync(board);

            return board;
        }
    }
}

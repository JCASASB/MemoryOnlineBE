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
        private readonly IGenericRepository<GameState> _gameRepository;

        public JoinGameHandler(
            IGenericRepository<GameState> gameRepository
            , IJoinGameUseCase joinGameUseCase)
        {
            _joinGameUseCase = joinGameUseCase;
            _gameRepository = gameRepository;
        }

        public async Task<GameState> Handle(JoinGameCommand request, CancellationToken cancellationToken)
        {
            var boards = await _gameRepository.GetAllAsync(g => g.Players, g => g.Cards);
            var board = boards.FirstOrDefault(g => g.Name == request.gameName);

            if (board == null)
                throw new KeyNotFoundException($"Game '{request.gameName}' not found");

            board = _joinGameUseCase.Execute(board, request.playerName);

            await _gameRepository.UpdateAsync(board);

            return board;
        }
    }
}

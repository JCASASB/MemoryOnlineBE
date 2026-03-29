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

        public Task<GameState> Handle(JoinGameCommand request, CancellationToken cancellationToken)
        {
            var board = _gameRepository.GetAll(includeProperties: "Players,Cards").FirstOrDefault(g => g.Name == request.gameName);

            if (board == null)
                throw new KeyNotFoundException($"Game '{request.gameName}' not found");

            board = _joinGameUseCase.Execute(board, request.playerName);

            // La entidad ya está siendo tracked desde GetAll(), solo Update() es necesario
            _gameRepository.Update(board);
            _gameRepository.SaveChanges();

            return Task.FromResult(board);
        }
    }
}

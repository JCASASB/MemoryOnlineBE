using MediatR;
using MemoryOnline.Domain.Entities;
using MemoryOnline.Repository.IRepository;

namespace MemoryOnline.Application.Application.Commands.UpdateGameState
{
    public class UpdateGameStateHandler : IRequestHandler<UpdateGameStateCommand, GameState>
    {
        private readonly IRepositoryGame _gameRepository;

        public UpdateGameStateHandler(IRepositoryGame gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public Task<GameState> Handle(UpdateGameStateCommand request, CancellationToken cancellationToken)
        {
            var board = _gameRepository.GetGameByName(request.gameState.Name);
            if (board == null)
            {
                _gameRepository.AddGame(request.gameState);
            }
            else
            {
                _gameRepository.UpdateGame(request.gameState);
            }
            return Task.FromResult(request.gameState);
        }
    }
}

using MediatR;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Commands.UpdateGameState
{
    public class UpdateGameStateHandler : IRequestHandler<UpdateGameStateCommand>
    {
        private readonly IGenericRepository<GameState> _gameRepository;

        public UpdateGameStateHandler(IGenericRepository<GameState> gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task Handle(UpdateGameStateCommand request, CancellationToken cancellationToken)
        {
            var boards = await _gameRepository.GetAllAsync(g => g.Players, g => g.Cards);
            var board = boards.FirstOrDefault(u => u.Name == request.gameState.Name);

            if (board == null)
            {
                await _gameRepository.AddAsync(request.gameState);
            }
            else
            {
                await _gameRepository.UpdateAsync(request.gameState);
            }
        }
    }
}

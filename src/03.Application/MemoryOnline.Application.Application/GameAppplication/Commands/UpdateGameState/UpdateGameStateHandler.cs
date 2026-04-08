using MediatR;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.Generic.IRepositories.Generic;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Commands.UpdateGameState
{
    public class UpdateGameStateHandler : IRequestHandler<UpdateGameStateCommand>
    {
        private readonly IGameRepository _gameRepository;

        public UpdateGameStateHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task Handle(UpdateGameStateCommand request, CancellationToken cancellationToken)
        {
            var boards = await _gameRepository.GetAllAsync();
            var board = boards.FirstOrDefault(g => g.Name == request.gameState.Name);

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

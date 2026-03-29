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
            var board = _gameRepository.GetAll(includeProperties: "Players,Cards").FirstOrDefault(u => u.Name == request.gameState.Name);

            if (board == null)
            {
                _gameRepository.Add(request.gameState);
            }
            else
            {
                _gameRepository.Attach(request.gameState);
                _gameRepository.Update(request.gameState);
            }

            _gameRepository.SaveChanges();

            await Task.CompletedTask;
        }
    }
}

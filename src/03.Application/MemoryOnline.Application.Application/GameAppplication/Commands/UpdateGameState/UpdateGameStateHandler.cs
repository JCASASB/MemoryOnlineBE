using MediatR;
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
            var board = _gameRepository.GetGameByName(request.gameState.Name);
            if (board == null)
            {
                _gameRepository.AddGame(request.gameState);
            }
            else
            {
                _gameRepository.UpdateGame(request.gameState);
            }

            await Task.CompletedTask;
        }
    }
}

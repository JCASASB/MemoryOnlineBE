using MediatR;
using MemoryOnline.Domain.Domain.IGameUseCases;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Commands.UpdateGameState
{
    public class AddNewStateHandler : IRequestHandler<AddNewStateCommand>
    {
        private readonly IMatchRepository _gameRepository;
        private readonly IAddNewStateUseCase _addNewStateUseCase;

        public AddNewStateHandler(
            IMatchRepository gameRepository
            , IAddNewStateUseCase addNewStateUseCase)
        {
            _gameRepository = gameRepository;
            _addNewStateUseCase = addNewStateUseCase;
        }

        public async Task Handle(AddNewStateCommand request, CancellationToken cancellationToken)
        {
            var match = await _gameRepository.GetMatchByNameAsync(request.gameState.Name);
            
            match = _addNewStateUseCase.Execute(match, request.gameState);

            await _gameRepository.UpdateAsync(match);
        }
    }
}

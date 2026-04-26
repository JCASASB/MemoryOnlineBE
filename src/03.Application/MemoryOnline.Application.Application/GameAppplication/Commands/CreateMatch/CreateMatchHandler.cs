using MediatR;
using MemoryOnline.Domain.Domain.IMatchUseCases;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Commands.CreateMatch
{
    public class CreateMatchHandler : IRequestHandler<CreateMatchCommand>
    {
        private readonly ICreateMatchUseCase _createMatchUseCase;
        private readonly IMatchRepository _matchRepository;

        public CreateMatchHandler(
            IMatchRepository matchRepository
            , ICreateMatchUseCase createMatchUseCase)
        {
            _createMatchUseCase = createMatchUseCase;
            _matchRepository = matchRepository;
        }

        public async Task Handle(CreateMatchCommand request, CancellationToken cancellationToken)
        {
            Match match = _createMatchUseCase.Execute(request.initialState, request.matchId);
            await _matchRepository.AddAsync(match);
        }
    }
}

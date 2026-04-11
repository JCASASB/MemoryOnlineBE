using MediatR;
using MemoryOnline.Domain.Domain.MatchUseCases;
using MemoryOnline.Domain.Entities.Game;
using MemoryOnline.Infraestructure.IRepository;

namespace MemoryOnline.Application.Application.GameAppplication.Commands.JoinGame
{
    public class JoinMatchHandler : IRequestHandler<JoinMatchCommand, BoardState>
    {
        private readonly IJoinMatchUseCase _joinMatchUseCase;
        private readonly IMatchRepository _gameRepository;

        public JoinMatchHandler(
            IMatchRepository gameRepository
            , IJoinMatchUseCase joinMatchUseCase)
        {
            _joinMatchUseCase = joinMatchUseCase;
            _gameRepository = gameRepository;
        }

        public async Task<BoardState> Handle(JoinMatchCommand request, CancellationToken cancellationToken)
        {
            var match = await _gameRepository.GetMatchByNameAsync(request.gameName);
      
            if (match == null)
                throw new KeyNotFoundException($"Game '{request.gameName}' not found");

            var board = match.States.FirstOrDefault() as BoardState;

            Console.WriteLine($"Game found: {board.Name}, Players count: {board.Players?.Count ?? 0}");

            board = _joinMatchUseCase.Execute(match, request.playerName);

            await _gameRepository.UpdateNewStateAsync(match.Id, board);

            return board;
        }
    }
}

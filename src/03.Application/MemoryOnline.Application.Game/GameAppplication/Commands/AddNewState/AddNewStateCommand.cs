using MediatR;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Application.Game.GameAppplication.Commands.UpdateGameState
{

    public record AddNewStateCommand(BoardState gameState, Guid matchId) : IRequest;
}

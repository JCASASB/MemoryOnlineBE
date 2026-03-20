using MediatR;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Application.Application.Commands.UpdateGameState
{

    public record UpdateGameStateCommand(GameState gameState) : IRequest;
}

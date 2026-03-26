using MediatR;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Application.Application.GameAppplication.Commands.UpdateGameState
{

    public record UpdateGameStateCommand(GameState gameState) : IRequest;
}

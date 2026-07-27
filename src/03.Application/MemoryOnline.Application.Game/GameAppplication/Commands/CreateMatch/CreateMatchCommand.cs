using MediatR;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Application.Game.GameAppplication.Commands.CreateMatch
{
    public record CreateMatchCommand(BoardState initialState, Guid idMatch, string name, int level) : IRequest;
}

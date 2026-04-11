using MediatR;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Application.Application.GameAppplication.Commands.CreateMatch
{
    public record CreateMatchCommand(BoardState initialState, Guid matchId) : IRequest;
}

using MediatR;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Application.Application.GameAppplication.Queries
{
    public record GetBoardStatesFromVersionQuery(Guid mathId, int version) : IRequest<List<BoardState>>;
}

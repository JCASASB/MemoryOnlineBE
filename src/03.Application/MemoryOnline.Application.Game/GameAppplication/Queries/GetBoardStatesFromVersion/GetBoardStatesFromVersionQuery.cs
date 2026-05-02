using MediatR;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Application.Game.GameAppplication.Queries.GetBoardStatesFromVersion
{
    public record GetBoardStatesFromVersionQuery(Guid mathId, int version) : IRequest<List<BoardState>>;
}

using MediatR;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Application.Game.GameAppplication.Queries.GetAllBoardStates
{
    public record GetAllBoardStatesQuery(Guid mathId) : IRequest<List<BoardState>>;
}

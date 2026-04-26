using MediatR;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Application.Application.GameAppplication.Queries
{
    public record GetAllBoardStatesQuery(Guid mathId) : IRequest<List<BoardState>>;
}

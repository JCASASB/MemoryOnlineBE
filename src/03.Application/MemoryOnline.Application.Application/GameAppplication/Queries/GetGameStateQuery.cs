using MediatR;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Application.Application.GameAppplication.Queries
{
    public record GetGameStateQuery(string gameName) : IRequest<BoardState>;
}

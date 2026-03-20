using MediatR;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Application.Application.Queries
{
    public record GetGameStateQuery(string gameName) : IRequest<GameState>;
}

using MediatR;
using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Application.Application.Queries
{
    public record GetGameStateQuery(string gameName) : IRequest<GameState>;
}

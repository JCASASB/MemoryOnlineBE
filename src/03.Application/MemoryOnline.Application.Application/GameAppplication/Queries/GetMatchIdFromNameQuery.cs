using MediatR;

namespace MemoryOnline.Application.Application.GameAppplication.Queries
{
    public record GetMatchIdFromNameQuery(string gameName) : IRequest<Guid>;
}

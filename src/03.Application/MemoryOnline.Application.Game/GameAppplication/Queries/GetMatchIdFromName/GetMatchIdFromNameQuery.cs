using MediatR;

namespace MemoryOnline.Application.Game.GameAppplication.Queries.GetMatchIdFromName
{
    public record GetMatchIdFromNameQuery(string gameName) : IRequest<Guid>;
}

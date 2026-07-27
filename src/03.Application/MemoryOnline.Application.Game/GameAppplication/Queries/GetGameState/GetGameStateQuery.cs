using MediatR;
using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Application.Game.GameAppplication.Queries.GetGameState
{
    public record GetGameStateQuery(string gameName) : IRequest<BoardState>;
}

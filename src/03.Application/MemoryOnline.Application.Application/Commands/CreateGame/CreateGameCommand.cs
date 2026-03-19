using MediatR;
using MemoryOnline.Domain.Entities;

namespace MemoryOnline.Application.Application.Commands.CreateGame
{
    public record CreateGameCommand(string PlayerName, string GameName, Guid GameId, int Level) : IRequest<GameState>;
}

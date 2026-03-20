using MediatR;

namespace MemoryOnline.Application.Application.Commands.CreateGame
{
    public record CreateGameCommand(string PlayerName, string GameName, Guid GameId, int Level) : IRequest;
}

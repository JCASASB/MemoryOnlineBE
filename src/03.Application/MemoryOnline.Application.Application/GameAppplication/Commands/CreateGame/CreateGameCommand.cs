using MediatR;

namespace MemoryOnline.Application.Application.GameAppplication.Commands.CreateGame
{
    public record CreateGameCommand(string PlayerName, string GameName, Guid GameId, int Level) : IRequest;
}

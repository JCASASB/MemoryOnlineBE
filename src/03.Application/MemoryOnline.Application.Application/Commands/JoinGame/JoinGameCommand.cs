namespace MemoryOnline.Application.Application.Commands.JoinGame
{
    using MediatR;
    using MemoryOnline.Domain.Entities;

    public record JoinGameCommand(string playerName, string gameName) : IRequest<GameState>;
}

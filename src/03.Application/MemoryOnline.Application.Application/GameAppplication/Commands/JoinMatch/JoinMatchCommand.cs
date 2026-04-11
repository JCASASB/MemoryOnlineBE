namespace MemoryOnline.Application.Application.GameAppplication.Commands.JoinGame
{
    using MediatR;
    using MemoryOnline.Domain.Entities.Game;

    public record JoinMatchCommand(string playerName, string gameName) : IRequest<BoardState>;
}

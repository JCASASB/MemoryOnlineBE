namespace MemoryOnline.Application.Application.GameAppplication.Commands.JoinGame
{
    using MediatR;
    using MemoryOnline.Domain.Entities.Game;

    public record JoinGameCommand(string playerName, string gameName) : IRequest<GameState>;
}

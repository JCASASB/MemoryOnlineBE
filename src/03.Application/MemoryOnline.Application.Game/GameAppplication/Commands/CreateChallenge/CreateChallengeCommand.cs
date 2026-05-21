using MediatR;

namespace MemoryOnline.Application.Game.GameAppplication.Commands.CreateChallenge
{
    public record CreateChallengeCommand(Guid matchId) : IRequest;

}

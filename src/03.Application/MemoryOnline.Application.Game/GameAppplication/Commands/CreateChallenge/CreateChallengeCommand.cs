using MediatR;

namespace MemoryOnline.Application.Game.GameAppplication.Commands.CreateChallenge
{
    public record CreateChallengeCommand(Guid matchId, Guid playerId1, Guid playerId2) : IRequest;

}

using MediatR;
using MemoryOnline.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Application.Application.Commands.UpdateGameState
{

    public record UpdateGameStateCommand(GameState gameState) : IRequest<GameState>;
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Domain.Entities.Stats
{
    public class UserMatchResult
    {
        public Guid Id { get; set; }
        public Guid MatchId { get; set; }
        public int Moves { get; set; }
        public int Fails { get; set; }
        public int Matchs { get; set; }
        public Boolean Winner { get; set; }

        public Guid UsuarioId { get; set; }
    }
}

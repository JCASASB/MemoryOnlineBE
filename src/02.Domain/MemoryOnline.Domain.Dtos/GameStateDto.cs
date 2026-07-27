using System;
using System.Collections.Generic;

namespace MemoryOnline.Dtos
{
    public class GameStateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CardDto> Cards { get; set; }
        public bool IsProcessing { get; set; }
        public List<PlayerDto> Players { get; set; }
    }

    public class PlayerDto
    {
        public Guid Id { get; set; }
        public int RemainMoves { get; set; } = 2;
        public int TotalMoves { get; set; } = 0;
    }

    public class CardDto
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public bool IsFlipped { get; set; } = false;
        public bool IsMatched { get; set; } = false;
    }
}

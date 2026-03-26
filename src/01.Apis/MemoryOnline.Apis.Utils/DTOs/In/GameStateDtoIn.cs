using System;
using System.Collections.Generic;

namespace MemoryOnline.Apis.Utils.DTOs.In
{
    public class GameStateDtoIn
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsProcessing { get; set; }
        public int Level { get; set; }
        public List<CardDtoIn> Cards { get; set; }
        public List<PlayerDtoIn> Players { get; set; }
    }
}


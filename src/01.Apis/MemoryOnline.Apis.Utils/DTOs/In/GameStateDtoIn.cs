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

        public class PlayerDtoIn
    {
            public string Id { get; set; }

            public string Name { get; set; }
            public int Points { get; set; }

            public Boolean Turn {  get; set; }
            public int RemainMoves { get; set; } 
            public int TotalMoves { get; set; } 
        }

        public class CardDtoIn
    {
            public string Id { get; set; }
            public string Value { get; set; }
            public string ImgUrl { get; set; }
            public bool IsRevealed { get; set; } 
            public bool IsMatched { get; set; } 
        }

    }

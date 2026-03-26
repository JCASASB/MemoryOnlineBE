using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Apis.Utils.DTOs.Out
{
    public class GameStateDtoOut
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public List<CardDtoOut>? cards { get; set; }
        public bool isProcessing { get; set; }
        public int level { get; set; }
        public List<PlayerDtoOut>? players { get; set; }
    }
}
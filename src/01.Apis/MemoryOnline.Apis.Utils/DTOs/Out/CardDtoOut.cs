using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Apis.Utils.DTOs.Out
{
    public class CardDtoOut
    {
        public string id { get; set; }
        public string value { get; set; }
        public string imgUrl { get; set; }
        public EnumCardState state { get; set; } 
    }
}

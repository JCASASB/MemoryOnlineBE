using MemoryOnline.Domain.Entities.Game;

namespace MemoryOnline.Apis.Utils.DTOs.In
{
    public class CardDtoIn
    {
        public string Id { get; set; }
        public int Value { get; set; }
        public string ImgUrl { get; set; }
        public EnumCardState State { get; set; }
    }
}

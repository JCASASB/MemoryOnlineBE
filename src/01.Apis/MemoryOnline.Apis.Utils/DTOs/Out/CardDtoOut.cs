namespace MemoryOnline.Apis.Utils.DTOs.Out
{
    public class CardDtoOut
    {
        public string id { get; set; }
        public string value { get; set; }
        public string imgUrl { get; set; }
        public bool isRevealed { get; set; } = false;
        public bool isMatched { get; set; } = false;
    }
}

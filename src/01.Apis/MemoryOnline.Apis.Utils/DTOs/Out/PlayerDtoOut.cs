namespace MemoryOnline.Apis.Utils.DTOs.Out
{
    public class PlayerDtoOut
    {
        public string id { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public int remainMoves { get; set; }
        public int totalMoves { get; set; }
        public bool turn { get; set; }
        public int points { get; set; }
    }
}

namespace MemoryOnline.Apis.Utils.DTOs.In
{
    public class PlayerDtoIn
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public bool Turn { get; set; }
        public int RemainMoves { get; set; }
        public int TotalMoves { get; set; }
    }
}

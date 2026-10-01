namespace MemoryOnline.Apis.Utils.DTOs.In
{
    public class PlayerDtoIn
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int Points { get; set; }
        public bool Turn { get; set; }
        public int RemainMoves { get; set; }
        public int TotalMoves { get; set; }
        public List<JokerDtoIn> Jokers { get; set; } = new();
    }

    public class JokerDtoIn
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
    }
}

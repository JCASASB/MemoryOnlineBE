
namespace MemoryOnline.Domain.Entities.Game
{
    /// <summary>
    /// POCO Entity 
    /// </summary>
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RemainMoves { get; set; }
        public int TotalMoves { get; set; }
        public int Points { get; set; }
        public bool Turn { get; set; }

        // Foreign Key hacia GameState
        public Guid GameStateId { get; set; }
        public GameState GameState { get; set; }
    }
}

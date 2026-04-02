
namespace MemoryOnline.Domain.Entities.Game
{
    /// <summary>
    /// POCO Entity - Representa el estado de un juego de Memory
    /// </summary>
    public class GameState
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Version { get; set; }
        public List<Card> Cards { get; set; } = new();
        public List<Player> Players { get; set; } = new();
    }
}

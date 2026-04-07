
namespace MemoryOnline.Domain.Entities.Game
{
    /// <summary>
    /// POCO Entity
    /// </summary>
    public class Card
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string ImgUrl { get; set; } = string.Empty;
        public EnumCardState State { get; set; }

        // Foreign Key hacia GameState
        public Guid GameStateId { get; set; }
        public GameState GameState { get; set; } 
    }
}

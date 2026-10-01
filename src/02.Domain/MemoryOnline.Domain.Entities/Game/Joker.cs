namespace MemoryOnline.Domain.Entities.Game
{
    /// <summary>
    /// Comodín perteneciente a un jugador.
    /// </summary>
    public class Joker
    {
        public Guid Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public int Status { get; set; }
    }
}

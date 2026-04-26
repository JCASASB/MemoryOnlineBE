
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MemoryOnline.Domain.Entities.Game
{
    /// <summary>
    /// POCO Entity - Embedded/Owned in GameState
    /// </summary>
    public class Player
    {
        [BsonElement("Id")]  // Esto cambia el nombre en JSON a "Id"
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public int RemainMoves { get; set; }
        public int TotalMoves { get; set; }
        public int Points { get; set; }
        public bool Turn { get; set; }
    }
}

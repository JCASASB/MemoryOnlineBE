
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoryOnline.Domain.Entities.Game
{
    /// <summary>
    /// POCO Entity - Embedded in GameState for MongoDB
    /// </summary>
    public class Player
    {
        [Key]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RemainMoves { get; set; }
        public int TotalMoves { get; set; }
        public int Points { get; set; }
        public bool Turn { get; set; }
        
        // Foreign Key hacia GameState
        [BsonIgnore]
        [NotMapped]
        public Guid GameStateId { get; set; }
       
        [BsonIgnore]
        [NotMapped]
        public GameState GameState { get; set; }
    }
}


using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoryOnline.Domain.Entities.Game
{
    /// <summary>
    /// POCO Entity - Embedded in GameState for MongoDB
    /// </summary>
    public class Card
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string ImgUrl { get; set; } = string.Empty;
        public EnumCardState State { get; set; }

        // Foreign Key hacia GameState
        [BsonIgnore]
        [NotMapped]
        public Guid GameStateId { get; set; }

        [BsonIgnore]
        [NotMapped]
        public GameState GameState { get; set; }
    }
}

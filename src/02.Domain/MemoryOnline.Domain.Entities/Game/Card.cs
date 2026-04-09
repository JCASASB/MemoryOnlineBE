
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MemoryOnline.Domain.Entities.Game
{
    /// <summary>
    /// POCO Entity - Embedded/Owned in GameState
    /// </summary>
    public class Card
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string ImgUrl { get; set; } = string.Empty;
        public EnumCardState State { get; set; }
    }
}

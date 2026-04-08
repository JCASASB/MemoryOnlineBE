
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MemoryOnline.Domain.Entities.Game
{
    /// <summary>
    /// POCO Entity for MongoDB - Players and Cards are embedded documents
    /// </summary>
    public class GameState
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Version { get; set; }

        public List<Card> Cards { get; set; } = new();

        public List<Player> Players { get; set; } = new();
    }
}

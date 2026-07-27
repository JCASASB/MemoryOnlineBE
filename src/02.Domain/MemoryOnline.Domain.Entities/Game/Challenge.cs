using MemoryOnline.Domain.Entities.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MemoryOnline.Domain.Entities.Game
{
    public class Challenge
    {
        [BsonId]
        [BsonElement("Id")]  // Esto cambia el nombre en JSON a "Id"
        [BsonRepresentation(BsonType.String)]
        [Key]
        public Guid Id { get; set; }
        public List<Player> Players { get; set; }
        public Match Match { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

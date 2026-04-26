using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MemoryOnline.Domain.Entities.Game
{
    /// <summary>
    /// POCO Entity 
    /// </summary>
    public class Match
    {
        [BsonId]
        [BsonElement("Id")]  // Esto cambia el nombre en JSON a "Id"
        [BsonRepresentation(BsonType.String)]
        [Key]
        public Guid Id { get; set; }

        public List<BoardState> States { get; set; }
    }
}

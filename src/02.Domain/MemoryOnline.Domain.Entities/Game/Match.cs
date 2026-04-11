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
        [Key]
        public Guid Id { get; set; }

        public List<BoardState> States { get; set; }
    }
}

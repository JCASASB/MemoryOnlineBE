using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MemoryOnline.Domain.Entities.Game
{
    /// <summary>
    /// POCO Entity 
    /// </summary>
    public class Match
    {
        [Key]
        public Guid Id { get; set; }

        public string State { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class PartialRound
    {
        [Key]
        public Guid Id { get; set; }
        public Location Location { get; set; }
        public Round Round { get; set; }
        public TimeSpan Duration { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Entities.Enumerations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Entities.Models
{
    public class Round
    {
        [Key]
        [Column("RoundId")]
        public Guid Id { get; set; }

        public Person Person { get; set; }
        [Required(ErrorMessage = "TotalKilometers required")]
        public int TotalKilometers { get; set; }
        public int TotalTimeInMinutes { get; set; }
        public double Temperature { get; set; }
        [EnumDataType(typeof(Style))]
        [Required(ErrorMessage = "Skistyle required. Insert 0 if not known.")]
        public int SkiStyle { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace grassroots.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string County { get; set; }
        
        [Required]
        public int Population { get; set; }

        [NotMapped]
        public double ManHours { get; set; }

        [NotMapped]
        public double adjustedPopulation { get; set; }
    }
}

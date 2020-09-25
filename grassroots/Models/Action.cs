using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace grassroots.Models
{
    public class Action
    {
        [Key]
        public int ActionId { get; set; }

        [Required]
        public int UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public int LocationId { get; set; }

        public Location Location { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

        [Required]
        public string City { get; set; }
    }
}

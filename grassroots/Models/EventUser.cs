using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace grassroots.Models
{
    public class EventUser
    {
        [Key]
        public int EventUserId { get; set; }

        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace grassroots.Models
{
    public class GatheringUser
    {
        [Key]
        public int GatheringUserId { get; set; }

        [Required]
        public int GatheringId { get; set; }
        public Gathering Gathering { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

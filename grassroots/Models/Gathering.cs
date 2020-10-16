using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace grassroots.Models
{
    public class Gathering
    {
        [Key]
        public int GatheringId { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Maximum Attendees")]
        public int? MaxAttendees { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Finish Time")]
        [GatheringDateChecker]
        public DateTime EndTime { get; set; }


        [Required]
        public int LocationId { get; set; }

        public Location Location { get; set; }

        [Required]
        public string City { get; set; }


        public List<GatheringUser> GatheringUsers { get; set; } = new List<GatheringUser>();
    }

    //Custom validator to make sure the Finish Time comes after the Start Time.
    public class GatheringDateChecker : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var gathering = (Gathering)validationContext.ObjectInstance;

            return (gathering.StartTime < gathering.EndTime)
                ? ValidationResult.Success
                : new ValidationResult("The finish time should come after the start time.");
        }
    }
}

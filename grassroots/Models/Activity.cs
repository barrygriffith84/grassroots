using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace grassroots.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public int LocationId { get; set; }

        public Location Location { get; set; }


        [Required]
        public string City { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Finish Time")]
        [ActivityDateChecker]
        public DateTime EndTime { get; set; }

    }

    //Custom validator to make sure the Finish Time comes after the Start Time.
    public class ActivityDateChecker : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var activity = (Activity)validationContext.ObjectInstance;

            return (activity.StartTime < activity.EndTime)
                ? ValidationResult.Success
                : new ValidationResult("The finish time should come after the start time.");
        }
    }
}


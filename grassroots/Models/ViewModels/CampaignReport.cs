using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grassroots.Models.ViewModels
{
    public class CampaignReport
    {
        public List<Location> locations { get; set; } = new List<Location>();

        public List<Activity> activities { get; set; } = new List<Activity>();

        public List<GatheringUser> gatheringUsers { get; set; } = new List<GatheringUser>();

        public double TotalCampaignHours { get; set; }

        public List<Location> locationsAdjustedPopulation { get; set; } = new List<Location>();
    }
}

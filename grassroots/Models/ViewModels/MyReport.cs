using grassroots.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grassroots.Models.ViewModels
{
    public class MyReport
    {
        public double MyHours { get; set; }

        public double TotalCampaignHours { get; set; }
        public double AverageUserHours { get; set; }

        public List<Activity> activities { get; set; } = new List<Activity>();

        public List<GatheringUser> gatheringUsers { get; set; } = new List<GatheringUser>();

        public List<Activity> myActivityHistory { get; set; } = new List<Activity>();

        public List<GatheringUser> myGatheringHistory { get; set; } = new List<GatheringUser>();


    }
}

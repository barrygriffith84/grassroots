using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grassroots.Models.ViewModels
{
    public class MySchedule
    {
        public List<Activity> activities { get; set; } = new List<Activity>();

        public List<Gathering> gatherings { get; set; } = new List<Gathering>();
        
    }
}

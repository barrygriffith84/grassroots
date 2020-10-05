using grassroots.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grassroots.Views.ViewModels
{
    public class MyReport
    {
        public double MyHours { get; set; }

        public double AverageUserHours { get; set; }

        public List<Activity> activities { get; set; } = new List<Activity>();

        public List<Gathering> gatherings { get; set; } = new List<Gathering>();

    }
}

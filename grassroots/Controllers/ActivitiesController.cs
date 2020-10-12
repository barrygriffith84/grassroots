using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using grassroots.Data;
using grassroots.Models;
using Microsoft.AspNetCore.Identity;
using grassroots.Models.ViewModels;

namespace grassroots.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ActivitiesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var applicationDbContext = _context.Activity.Include(a => a.Location).Include(a => a.User)
                .Where(a => a.UserId == user.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .Include(a => a.Location)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (activity == null)
            {
                return NotFound();

            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "County");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityId,UserId,LocationId,City,Title,Description,StartTime,EndTime")] Activity activity)
        {

            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                activity.UserId = user.Id;
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "County", activity.LocationId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", activity.UserId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "County", activity.LocationId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", activity.UserId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActivityId,UserId,LocationId,City,Title,Description,StartTime,EndTime")] Activity activity)
        {
            if (id != activity.ActivityId)
            {
                return NotFound();
            }

            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await GetCurrentUserAsync();
                    activity.UserId = user.Id;
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.ActivityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "County", activity.LocationId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", activity.UserId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .Include(a => a.Location)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activity.FindAsync(id);
            _context.Activity.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Controller to get all Actions and GatheringUsers
        public async Task<IActionResult> MyReport()
        {
            var user = await GetCurrentUserAsync();

            MyReport myReport = new MyReport
            {
                activities = await _context.Activity.Include(a => a.Location).Where(a => a.EndTime < DateTime.Now).ToListAsync(),

                gatheringUsers = await _context.GatheringUser.Include(gu => gu.Gathering).ThenInclude(g => g.Location).Where(gu => gu.Gathering.EndTime < DateTime.Now).ToListAsync()
            };

            List<string> users = new List<string>();

            //Get all of the activities that have ended for the user that is logged in.
            myReport.myActivityHistory = myReport.activities.Where(a => a.UserId == user.Id).ToList();

            //Get all of the gatherings the logged-in user has attended that have ended.
            myReport.myGatheringHistory = myReport.gatheringUsers.Where(gu => gu.UserId == user.Id).ToList();

            //For each activity, get the hours between the start time and end time and store them in the TotalCampaignHours property.
            myReport.activities.ForEach(a =>
            {
                TimeSpan diff = a.EndTime - a.StartTime;
                double hours = diff.TotalHours;
                users.Add(a.UserId);

                if(a.UserId == user.Id)
                {
                    myReport.MyHours += hours;
                }

                myReport.TotalCampaignHours += hours;
            });

            myReport.gatheringUsers.ForEach( gu =>
            {
                TimeSpan diff = gu.Gathering.EndTime - gu.Gathering.StartTime;
                double hours = diff.TotalHours;
                       users.Add(gu.UserId);

                if (gu.UserId == user.Id)
                {
                     myReport.MyHours += hours;
                }

                myReport.TotalCampaignHours += hours;
            });

            //Get the number of distinct users that are active(Have completed at an Activity or attended a Gathering).
            List<string> distinctUsers = users.Distinct().ToList();
            int ActiveUsersCount = distinctUsers.Count();

            myReport.MyHours = Math.Round(myReport.MyHours,2);
            myReport.AverageUserHours = Math.Round(myReport.TotalCampaignHours / ActiveUsersCount, 2);
              
            return View(myReport);
        }

        public async Task<IActionResult> CampaignReport()
        {
            CampaignReport campaignReport = new CampaignReport
            {
                //Get the locations, activities, and gatheringUsers from the database
                locations = await _context.Location.ToListAsync(),
                activities = await _context.Activity.Include(a => a.Location).Where(a => a.EndTime < DateTime.Now).ToListAsync(),
                gatheringUsers = await _context.GatheringUser.Include(gu => gu.Gathering).ThenInclude(g => g.Location).Where(gu => gu.Gathering.EndTime < DateTime.Now).ToListAsync()
            };

            //Go through each Activity and add the hours to the manhours property in the Location for the activity.
            campaignReport.activities.ForEach(a => campaignReport.locations.FirstOrDefault(l => l.LocationId == a.LocationId).ManHours += (a.EndTime - a.StartTime).TotalHours);

            //var test = campaignReport.activities[0].EndTime - campaignReport.activities[0].StartTime;
            campaignReport.gatheringUsers.ForEach(gu => campaignReport.locations.FirstOrDefault(l => l.LocationId == gu.Gathering.LocationId).ManHours += (gu.Gathering.EndTime - gu.Gathering.StartTime).TotalHours);

            var test = (campaignReport.gatheringUsers[0].Gathering.EndTime - campaignReport.gatheringUsers[0].Gathering.StartTime).TotalHours;

            campaignReport.locations.ForEach(l => l.adjustedPopulation = l.Population - l.ManHours * 5);

            campaignReport.locations.ForEach(l => {
                l.adjustedPopulation = Math.Round(l.adjustedPopulation);
                l.ManHours = Math.Round(l.ManHours,2); 
            });

            return View(campaignReport);
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.ActivityId == id);
        }
    }
}

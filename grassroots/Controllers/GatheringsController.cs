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
using Microsoft.EntityFrameworkCore.Query;

namespace grassroots.Controllers
{
    public class GatheringsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GatheringsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: Gatherings created by the user
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, string value)
        {
            //Sorts.  It sorts by start time by default.
            ViewData["CurrentSort"] = sortOrder;
            ViewData["StartSortParm"] = String.IsNullOrEmpty(sortOrder) ? "start_desc" : "";
            ViewData["CountySortParm"] = sortOrder == "county_desc" ? "county_asc" : "county_desc";
            ViewData["CitySortParm"] = sortOrder == "city_desc" ? "city_asc" : "city_desc";
            ViewData["TitleSortParm"] = sortOrder == "title_desc" ? "title_asc" : "title_desc";
            ViewData["AttendeesSortParm"] = sortOrder == "attendees_desc" ? "attendees_asc" : "attendees_desc";
            ViewData["FinishSortParm"] = sortOrder == "finish_desc" ? "finish_asc" : "finish_desc";
           
            //Used for conditinal rendering this method and the index view
            ViewData["Value"] = value;

            //The user that's currently logged in
            var user = await GetCurrentUserAsync();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            IQueryable<Gathering> applicationDbContext = _context.Gathering
                .Include(g => g.Location)
                .Include(g => g.User)
                .Include(g => g.GatheringUsers);
                
            if (value == "join") //Return Gatherings the logged-in user hasn't joined
            {
                applicationDbContext = applicationDbContext.Where(g => /*g.StartTime > DateTime.Now &&*/ !g.GatheringUsers.Any(gu => gu.UserId == user.Id));
            }
            else if (value == "joined") //Return Gatherings the logged-in user has joined
            {
                applicationDbContext = applicationDbContext.Where(g => g.GatheringUsers.Any(gu => gu.UserId == user.Id));
            }
            else //Return Gatherings the user has created
            {
                applicationDbContext = applicationDbContext.Where(g => g.UserId == user.Id);
            }
          
            //Search by county
            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(g => g.Location.County.Contains(searchString));
            }

            //Table sorts
            switch (sortOrder)
            {
                case "county_asc":
                    applicationDbContext = applicationDbContext.OrderBy(g => g.Location.County);
                    break;

                case "county_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(g => g.Location.County);
                    break;

                case "city_asc":
                    applicationDbContext = applicationDbContext.OrderBy(g => g.City);
                    break;

                case "city_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(g => g.City);
                    break;

                case "title_asc":
                    applicationDbContext = applicationDbContext.OrderBy(g => g.Title);
                    break;

                case "title_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(g => g.Title);
                    break;

                case "attendees_asc":
                    applicationDbContext = applicationDbContext.OrderBy(g => g.MaxAttendees);
                    break;

                case "attendees_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(g => g.MaxAttendees);
                    break;

                case "start_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(g => g.StartTime);
                    break;

                case "finish_asc":
                    applicationDbContext = applicationDbContext.OrderBy(g => g.EndTime);
                    break;

                case "finish_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(g => g.EndTime);
                    break;

                default:
                    applicationDbContext = applicationDbContext = applicationDbContext.OrderBy(g => g.StartTime);
                    break;
            }
            //Number of rows printed per page
            int pageSize = 5;

            return View(await PaginatedList<Gathering>.CreateAsync(applicationDbContext.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // Get: All Events
        public async Task<IActionResult> AllEvents()
        {
            var user = await GetCurrentUserAsync();
            var applicationDbContext = _context.Gathering.Include(g => g.Location).Include(g => g.User)
                .Include(g => g.GatheringUsers).Where(g => g.StartTime > DateTime.Now && !g.GatheringUsers.Any(gu => gu.UserId == user.Id) && g.UserId != user.Id);
                
            return View(await applicationDbContext.ToListAsync());
        }

        // Get: Events User Has Joined
        public async Task<IActionResult> JoinedEvents()
        {
            var user = await GetCurrentUserAsync();

            var applicationDbContext = _context.GatheringUser.Include(g => g.Gathering).Where(g => g.UserId == user.Id);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Gatherings/Details/5
        public async Task<IActionResult> Details(int? id, string detailType)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["DetailType"] = detailType; 

            var gathering = await _context.Gathering
                .Include(g => g.Location)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GatheringId == id);
            if (gathering == null)
            {
                return NotFound();
            }

            return View(gathering);
        }

        // GET: Gatherings/AllGathering/Join/5
        public async Task<IActionResult> Join(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gathering = await _context.Gathering
                .Include(g => g.Location)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GatheringId == id);
            if (gathering == null)
            {
                return NotFound();
            }

            return View(gathering);
        }


        // GET: Gatherings/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "County");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id");
            return View();
        }

        // POST: Gatherings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GatheringId,UserId,Title,Description,MaxAttendees,StartTime,EndTime,LocationId,City")] Gathering gathering)
        {
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                gathering.UserId = user.Id;
                _context.Add(gathering);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "County", gathering.LocationId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", gathering.UserId);
            return View(gathering);
        }

        // GET: Gatherings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gathering = await _context.Gathering.FindAsync(id);
            if (gathering == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "County", gathering.LocationId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", gathering.UserId);
            return View(gathering);
        }

        // POST: Gatherings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GatheringId,UserId,Title,Description,MaxAttendees,StartTime,EndTime,LocationId,City")] Gathering gathering)
        {
            if (id != gathering.GatheringId)
            {
                return NotFound();
            }

            ModelState.Remove("UserId");
            var user = await GetCurrentUserAsync();
            gathering.UserId = user.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gathering);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GatheringExists(gathering.GatheringId))
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
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "County", gathering.LocationId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", gathering.UserId);
            return View(gathering);
        }

        // GET: Gatherings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gathering = await _context.Gathering
                .Include(g => g.Location)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GatheringId == id);
            if (gathering == null)
            {
                return NotFound();
            }

            return View(gathering);
        }

        // POST: Gatherings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gathering = await _context.Gathering.FindAsync(id);
            _context.Gathering.Remove(gathering);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: GatheringUsers/Delete/5
        public async Task<IActionResult> LeaveGathering(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gatheringUser = await _context.GatheringUser
                .Include(g => g.Gathering)
                .ThenInclude(g => g.Location)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GatheringUserId == id);
           
            if (gatheringUser == null)
            {
                return NotFound();
            }

            return View(gatheringUser);
        }

        // POST: GatheringUsers/Delete/5
        [HttpPost, ActionName("LeaveGathering")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveGatheringConfirmed(int id)
        {
            var gatheringUser = await _context.GatheringUser.FindAsync(id);
            _context.GatheringUser.Remove(gatheringUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> JoinGathering(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var gathering = await _context.Gathering
                .Include(g => g.Location)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GatheringId == id);
            if (gathering == null)
            {
                return NotFound();
            }

            var user = await GetCurrentUserAsync();

            GatheringUser gatheringUser = new GatheringUser()
            {
                UserId = user.Id,
                GatheringId = gathering.GatheringId
            };

            if (ModelState.IsValid)
            {
                _context.Add(gatheringUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));

            
        }


        private bool GatheringExists(int id)
        {
            return _context.Gathering.Any(e => e.GatheringId == id);
        }
    }
}

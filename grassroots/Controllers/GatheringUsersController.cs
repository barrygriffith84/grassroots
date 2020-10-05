using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using grassroots.Data;
using grassroots.Models;

namespace grassroots.Controllers
{
    public class GatheringUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GatheringUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GatheringUsers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GatheringUser.Include(g => g.Gathering).Include(g => g.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GatheringUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gatheringUser = await _context.GatheringUser
                .Include(g => g.Gathering)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GatheringUserId == id);
            if (gatheringUser == null)
            {
                return NotFound();
            }

            return View(gatheringUser);
        }

        // GET: GatheringUsers/Create
        public IActionResult Create()
        {
            ViewData["GatheringId"] = new SelectList(_context.Gathering, "GatheringId", "City");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id");
            return View();
        }

        // POST: GatheringUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GatheringUserId,GatheringId,UserId")] GatheringUser gatheringUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gatheringUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GatheringId"] = new SelectList(_context.Gathering, "GatheringId", "City", gatheringUser.GatheringId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", gatheringUser.UserId);
            return View(gatheringUser);
        }

        // GET: GatheringUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gatheringUser = await _context.GatheringUser.FindAsync(id);
            if (gatheringUser == null)
            {
                return NotFound();
            }
            ViewData["GatheringId"] = new SelectList(_context.Gathering, "GatheringId", "City", gatheringUser.GatheringId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", gatheringUser.UserId);
            return View(gatheringUser);
        }

        // POST: GatheringUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GatheringUserId,GatheringId,UserId")] GatheringUser gatheringUser)
        {
            if (id != gatheringUser.GatheringUserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gatheringUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GatheringUserExists(gatheringUser.GatheringUserId))
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
            ViewData["GatheringId"] = new SelectList(_context.Gathering, "GatheringId", "City", gatheringUser.GatheringId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", gatheringUser.UserId);
            return View(gatheringUser);
        }

        // GET: GatheringUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gatheringUser = await _context.GatheringUser
                .Include(g => g.Gathering)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GatheringUserId == id);
            if (gatheringUser == null)
            {
                return NotFound();
            }

            return View(gatheringUser);
        }

        // POST: GatheringUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gatheringUser = await _context.GatheringUser.FindAsync(id);
            _context.GatheringUser.Remove(gatheringUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GatheringUserExists(int id)
        {
            return _context.GatheringUser.Any(e => e.GatheringUserId == id);
        }
    }
}

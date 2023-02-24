using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.Data;
using SteamProject.Models;

namespace SteamProject.Controllers
{
    public class SteamInfoUsersController : Controller
    {
        private IUserRepository _userRepo;

        public SteamInfoUsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // GET: SteamInfoUsers
        public async Task<IActionResult> Index()
        {
              return _userRepo.GetAll() != null ? 
                          View(_userRepo.GetAll().ToList()) :
                          Problem("Entity set 'SteamInfoDbContext.Users'  is null.");
        }

        // GET: SteamInfoUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _userRepo.GetAll() == null)
            {
                return NotFound();
            }

            var user = await _userRepo.GetAll()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: SteamInfoUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SteamInfoUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SteamId,SteamName,ProfileUrl,AvatarUrl,PersonaState,PlayerLevel")] User user)
        {
            if (ModelState.IsValid)
            {
                _userRepo.AddOrUpdate(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: SteamInfoUsers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _userRepo.GetAll() == null)
            {
                return NotFound();
            }

            var user = _userRepo.FindById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: SteamInfoUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SteamId,SteamName,ProfileUrl,AvatarUrl,PersonaState,PlayerLevel")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _userRepo.AddOrUpdate(user);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: SteamInfoUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _userRepo.GetAll() == null)
            {
                return NotFound();
            }

            var user = await _userRepo.GetAll()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: SteamInfoUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_userRepo.GetAll() == null)
            {
                return Problem("Entity set 'SteamInfoDbContext.Users'  is null.");
            }
            var user = _userRepo.FindById(id);
            if (user != null)
            {
                _userRepo.Delete(user);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_userRepo.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Data;
using SteamProject.Models;

namespace SteamProject.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepository;

        public UserController(UserManager<IdentityUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }



        // GET: SteamInfoUsers
        public async Task<IActionResult> Index()
        {
              return _userRepository.GetAll() != null ? 
                          View(_userRepository.GetAll().ToList()) :
                          Problem("Entity set 'SteamInfoDbContext.Users'  is null.");
        }

        // GET: SteamInfoUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _userRepository.GetAll() == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetAll()
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
                _userRepository.AddOrUpdate(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: SteamInfoUsers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _userRepository.GetAll() == null)
            {
                return NotFound();
            }

            var user = _userRepository.FindById(id);
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
                    _userRepository.AddOrUpdate(user);
                    
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
            if (id == null || _userRepository.GetAll() == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetAll()
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
            if (_userRepository.GetAll() == null)
            {
                return Problem("Entity set 'SteamInfoDbContext.Users'  is null.");
            }
            var user = _userRepository.FindById(id);
            if (user != null)
            {
                _userRepository.Delete(user);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_userRepository.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

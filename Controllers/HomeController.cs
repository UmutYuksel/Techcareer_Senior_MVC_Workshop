using EfCore.Models;
using EfCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using EfCore.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EfCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Index - Kullanıcılar ve Görevleri Görüntüleme
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Include(u => u.Duties).ToListAsync();
            var duties = await _context.Dutys.Include(d => d.User).ToListAsync();

            var viewModel = new HomeIndexViewModel
            {
                Users = users,
                Duties = duties
            };

            return View(viewModel);
        }

        // User Create - Kullanıcı Ekleme
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // User Edit - Kullanıcı Düzenleme
        public async Task<IActionResult> EditUser(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(Guid id, User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }


        // User Delete - Kullanıcı Silme
        public async Task<IActionResult> DeleteUser(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Duty Create - Görev Ekleme
        public IActionResult CreateDuty()
        {
            ViewData["Users"] = new SelectList(_context.Users, "UserId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDuty(Duty duty)
        {
            if (ModelState.IsValid)
            {
                _context.Dutys.Add(duty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Users"] = new SelectList(_context.Users, "UserId", "Name", duty.UserId);
            return View(duty);
        }

        // Duty Edit - Görev Düzenlem
        [HttpGet]
        public async Task<IActionResult> EditDuty(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duty = await _context.Dutys.FindAsync(id);
            if (duty == null)
            {
                return NotFound();
            }
            ViewData["Users"] = new SelectList(_context.Users, "UserId", "Name", duty.UserId);
            return View(duty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDuty(int id, Duty duty)
        {
            if (id != duty.DutyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(duty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DutyExists(duty.DutyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["Users"] = new SelectList(_context.Users, "UserId", "Name", duty.UserId);
            return View(duty);
        }


        // Duty Delete - Görev Silme
        public async Task<IActionResult> DeleteDuty(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duty = await _context.Dutys
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DutyId == id);
            if (duty == null)
            {
                return NotFound();
            }

            return View(duty);
        }

        [HttpPost, ActionName("DeleteDuty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDutyConfirmed(int id)
        {
            var duty = await _context.Dutys.FindAsync(id);
            _context.Dutys.Remove(duty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Yardımcı Metodlar - Kullanıcı ve Görev Varlığı Kontrolü
        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private bool DutyExists(int id)
        {
            return _context.Dutys.Any(e => e.DutyId == id);
        }
    }
}

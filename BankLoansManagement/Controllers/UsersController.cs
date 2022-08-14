using BankLoansManagement.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankLoansManagement.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BankLoansManagement.Models;

namespace BankLoansManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly BankLoanContext _context;
        public UsersController(BankLoanContext context)
        {
            _context = context;
        }

        // GET: UsersController
        public async Task<ActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();

            var userVM = new UserViewModel { Users = users };

            return View(userVM);
        }
        
        // Search for user
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var users = await _context.Users.ToListAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.FirstName.Contains(searchString)).ToList();
            }

            var usersVM = new UserViewModel
            {
                Users = users
            };

            return View(usersVM);
        }
        
        // GET: UsersController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            return View(user);
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            return View(user);
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, User newUserInfo)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                user.FirstName = newUserInfo.FirstName;
                user.LastName = newUserInfo.LastName;

                _context.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            return View(user);
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                
                _context.Remove(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

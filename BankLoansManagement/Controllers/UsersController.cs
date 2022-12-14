using BankLoansManagement.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankLoansManagement.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BankLoansManagement.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace BankLoansManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly BankLoanContext _context;
        private readonly ILogger<UsersController> _logger;
        public UsersController(BankLoanContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;   
        }

        // GET: UsersController
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            
            return Json(new { data = users });
        }

        // GET: UsersController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                return View(user);
            }
            catch(Exception e)
            {
                _logger.Log(LogLevel.Error, $"Failed to fetch user details for userId={id}, error={e.Message}", e);
                return RedirectToAction(nameof(Index));
            }
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

                    _logger.Log(LogLevel.Debug, $"A create new user Firstname={user.FirstName}, Lastname={user.LastName}, was created successfully");
                }
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                _logger.Log(LogLevel.Error, $"Failed to create new user Firstname={user.FirstName}, Lastname={user.LastName}, error={e.Message}", e);
                return View();
            }
        }

        // GET: UsersController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                if (user != null)
                {
                    return View(user);
                }
                return View(new User());
            }
            catch(Exception ex) 
            {
                _logger.Log(LogLevel.Error, $"Unexpected error occured, error={ex.Message}", ex);
                return View(new User());
            }
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, User user)
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();

                _logger.Log(LogLevel.Debug, $"UserId={user.UserId}, FirstName={user.FirstName}, LastName={user.LastName} was edited successfuly");
                transaction.Commit();       //All good: Commit to DB

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                _logger.Log(LogLevel.Error, $"Editing UserId = {user.UserId}, FirstName={user.FirstName}, LastName={user.LastName}, has failed, error = {e.Message}", e);
                transaction.Rollback();    //Undo DB transactions
                return View();
            }
        }

        // POST: UsersController/Delete/5
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                var userLoans = await _context.Loans.Where(l=> l.UserId==user.UserId).ToListAsync();
                
                _context.Remove(user);
                _context.RemoveRange(userLoans);
                await _context.SaveChangesAsync();

                _logger.Log(LogLevel.Debug, $"UserId={id}, FirstName={user.FirstName}, LastName={user.LastName} was deleted successfuly");
                transaction.Commit();       //All good: Commit to DB
                
                return Json(new { success = true, message = "Delete successful" });
            }
            catch(Exception e)
            {
                transaction.Rollback();    //Undo DB transactions
                _logger.Log(LogLevel.Error, $"Delete UserId = {id} failed, error = {e.Message}", e);
                
                return Json(new { success = true, message = "Delete failed, something went wrong" });
            }
        }







        /*===========================================  STRICTLY FOR DEMO PURPOSE =========================================================*/
        /*                                                                                                                                */
        /*                                                                                                                                */
        /*                                                                                                                                */
        /*                                                                                                                                */


        //For DEMO purpose, create random strings for firstNames and lastName
        private static Random random = new Random();
        public static string RandomString(int length, bool numbersOnly = false)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            if (numbersOnly)
            {
                chars = "0123456789";
            }
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // For DEMO purpose, create random 10 new users
        [HttpPost]
        public async Task<ActionResult> CreateRandom()
        {
            try
            {
                var users = new List<User>();
                for (int i = 0; i < 10; i++)
                {
                    var user = new User
                    {
                        FirstName = RandomString(random.Next(5, 10)),
                        LastName = RandomString(random.Next(5, 10)),
                        IdNumber = RandomString(13, true)
                    };
                    users.Add(user);
                }
                _context.AddRange(users);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<ActionResult> DeleteAllLoans()
        {
            try
            {
                var allLoans = await _context.Loans.ToListAsync();
                _context.RemoveRange(allLoans);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAllUsers()
        {
            try
            {
                var allUsers = await _context.Users.ToListAsync();
                _context.RemoveRange(allUsers);
                await _context.SaveChangesAsync();

                await DeleteAllLoans();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

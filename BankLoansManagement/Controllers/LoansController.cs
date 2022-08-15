using BankLoansManagement.Data;
using BankLoansManagement.Models;
using BankLoansManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BankLoansManagement.Helpers;
using System.Collections.Generic;

namespace BankLoansManagement.Controllers
{
    public class LoansController : Controller
    {

        private readonly BankLoanContext _context;
        public LoansController(BankLoanContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var loans = await _context.Loans.ToListAsync();
            var allUsers = await _context.Users.ToListAsync();

            var userLoansVm = new List<UserLoanViewModel>();

            foreach (var loan in loans)
            {
                var user = allUsers.FirstOrDefault(u => u.UserId == loan.UserId);
                if (user != null)
                {
                    var userLoan = new UserLoanViewModel
                    {
                        LoanId = loan.Id,
                        LoanAmount = loan.Amount,
                        LoanInterestRate = loan.InterestRate,
                        LoanType = loan.Type,
                        LoanTotalAmount = loan.Total,
                        ClientFirstName = user.FirstName,
                        ClientLastName = user.LastName,
                        ClientIdNumber = user.IdNumber
                    };
                    userLoansVm.Add(userLoan);
                }
            }
            return Json(new { data = userLoansVm });
        }
       
        // GET: LoansController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var loan = await _context.Loans.FirstOrDefaultAsync(u=> u.Id==id);
            return View(loan);
        }

        // GET: LoansController/Create
        public ActionResult Create(int userId)
        {
            var userLoan = new UserLoanViewModel
            {
                UserId = userId
            };
            return View(userLoan);
        }

        // POST: LoansController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserLoanViewModel newLoanApplication)
        {
            try
            {
                var loan = new Loan
                {
                    UserId = newLoanApplication.UserId,
                    Amount = newLoanApplication.LoanAmount,
                    InterestRate = newLoanApplication.LoanInterestRate,
                    Term = newLoanApplication.LoanTerm,
                    Type = newLoanApplication.LoanType
                };

                //New loan for new user => Create new user in db first
                if (loan.UserId == -1)
                {
                    var user = new User
                    {
                        FirstName = newLoanApplication.ClientFirstName,
                        LastName = newLoanApplication.ClientLastName,
                        IdNumber = newLoanApplication.ClientIdNumber
                    };

                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    loan.UserId = (await _context.Users.FirstOrDefaultAsync(u => u.IdNumber == user.IdNumber)).UserId;
                }

                if(loan.Type == EnumsHelpers.GetDisplayName(EnumsHelpers.LoanType.Personal))
                {
                    loan.Total = loan.Amount + loan.Amount * 0.25; 
                }
                else if(loan.Type == EnumsHelpers.GetDisplayName(EnumsHelpers.LoanType.Home))
                {
                    loan.Total = loan.Amount + loan.Amount * 0.10;
                }
                else if(loan.Type == EnumsHelpers.GetDisplayName(EnumsHelpers.LoanType.Vehicle))
                {
                    loan.Total = loan.Amount + loan.Amount * 0.15;
                }

                _context.Add(loan);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoansController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var loan = await _context.Loans.FirstOrDefaultAsync(u => u.Id == id);
            return View(loan);
        }

        // POST: LoansController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Loan loan)
        {
            try
            {
                if (loan.Type == EnumsHelpers.GetDisplayName(EnumsHelpers.LoanType.Personal))
                {
                    loan.Total = loan.Amount + loan.Amount * 0.25;
                }
                else if (loan.Type == EnumsHelpers.GetDisplayName(EnumsHelpers.LoanType.Home))
                {
                    loan.Total = loan.Amount + loan.Amount * 0.10;
                }
                else if (loan.Type == EnumsHelpers.GetDisplayName(EnumsHelpers.LoanType.Vehicle))
                {
                    loan.Total = loan.Amount + loan.Amount * 0.15;
                }
                _context.Update(loan);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //// GET: LoansController/Delete/5
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var loan = await _context.Loans.FirstOrDefaultAsync(u => u.Id == id);
        //    return View(loan);
        //}

        // POST: LoansController/Delete/5
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var loan = await _context.Loans.FirstOrDefaultAsync(u => u.Id == id);
                _context.Remove(loan);

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Delete successful" });
            }
            catch
            {
                return Json(new { success = true, message = "Delete failed" });
            }
        }
    }
}

using BankLoansManagement.Data;
using BankLoansManagement.Models;
using BankLoansManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BankLoansManagement.Helpers;

namespace BankLoansManagement.Controllers
{
    public class LoansController : Controller
    {

        private readonly BankLoanContext _context;
        public LoansController(BankLoanContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            var loans = await _context.Loans.ToListAsync();
            
            var userLoanVM = new UserLoanViewModel 
            { 
                userLoans = loans 
            };
            
            return View(userLoanVM);
        }

        // Search for loans
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var loans = await _context.Loans.ToListAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                loans = loans.Where(l => l.Type.Contains(searchString)).ToList();
            }

            var userLoansVM = new UserLoanViewModel
            {
                userLoans = loans
            };

            return View(userLoansVM);
        }

        // GET: LoansController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var loan = await _context.Loans.FirstOrDefaultAsync(u=> u.Id==id);
            return View(loan);
        }

        // GET: LoansController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoansController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Loan loan)
        {
            try
            {
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
                _context.Update(loan);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoansController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var loan = await _context.Loans.FirstOrDefaultAsync(u => u.Id == id);
            return View(loan);
        }

        // POST: LoansController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

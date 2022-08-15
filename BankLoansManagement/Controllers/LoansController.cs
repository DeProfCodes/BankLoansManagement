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
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;

namespace BankLoansManagement.Controllers
{
    public class LoansController : Controller
    {

        private readonly BankLoanContext _context;
        private readonly ILogger<UsersController> _logger;

        public LoansController(BankLoanContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
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
            catch(Exception e)
            {
                _logger.Log(LogLevel.Error, $"Failed to fetch all loans, error={e.Message}", e);
                return Json(new { data = new List<UserLoanViewModel>()});
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTotals()
        {
            try
            {
                var loans = await _context.Loans.ToListAsync();

                var totals = loans.Sum(l => l.Total);
                var amounts = loans.Sum(l => l.Amount);
                var fees = totals - amounts;

                var loanTotalsViewModel = new LoanTotalsVieweModel
                {
                    LoanTotals = totals,
                    LoanFeesTotal = fees
                };

                return Json(new { data = loanTotalsViewModel });
            }
            catch(Exception e)
            {
                _logger.Log(LogLevel.Error, $"Failed to fetch loan totals, error={e.Message}", e);
                return Json(new { data = new LoanTotalsVieweModel { LoanFeesTotal = 0, LoanTotals = 0 }});
            }
        }
        // GET: LoansController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var loan = await _context.Loans.FirstOrDefaultAsync(u => u.Id == id);
                return View(loan);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, $"Failed to fetch loan details for loanId={id}, error={e.Message}", e);
                return RedirectToAction(nameof(Index));
            }
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

        private double GetLoanTotal(Loan loan)
        {
            double total = 0;
            if (loan.Type == EnumsHelpers.GetDisplayName(EnumsHelpers.LoanType.Personal))
            {
                total = loan.Amount + loan.Amount * 0.25;
            }
            else if (loan.Type == EnumsHelpers.GetDisplayName(EnumsHelpers.LoanType.Home))
            {
                total = loan.Amount + loan.Amount * 0.10;
            }
            else if (loan.Type == EnumsHelpers.GetDisplayName(EnumsHelpers.LoanType.Vehicle))
            {
                total = loan.Amount + loan.Amount * 0.15;
            }
            return total;
        }

        // POST: LoansController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserLoanViewModel newLoanApplication)
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
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

                var loanUser = new User();
                //New loan for new user => Create new user in db first
                if (loan.UserId == -1)
                {
                    var user = new User
                    {
                        FirstName = newLoanApplication.ClientFirstName,
                        LastName = newLoanApplication.ClientLastName,
                        IdNumber = newLoanApplication.ClientIdNumber
                    };

                    loanUser = user;
                    
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    loanUser = await _context.Users.FirstOrDefaultAsync(u => u.IdNumber == user.IdNumber);
                    loan.UserId = loanUser.UserId;
                }

                loan.Total = GetLoanTotal(loan);

                _context.Add(loan);
                await _context.SaveChangesAsync();

                _logger.Log(LogLevel.Debug, $"A new loan of type={loan.Type} was created for user: userId={loanUser.UserId}, FirstName={loanUser.FirstName}, LastName={loanUser.LastName}, ID Number={loanUser.IdNumber} was created successfuly");
                transaction.Commit();       //All good: Commit to DB

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                transaction.Rollback();    //Undo DB transactions
                _logger.Log(LogLevel.Error, $"A loan of type={newLoanApplication.LoanType}, amount={newLoanApplication.LoanAmount} could not be created, error = {e.Message}", e);

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
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                loan.Total = GetLoanTotal(loan);

                _context.Update(loan);
                await _context.SaveChangesAsync();

                _logger.Log(LogLevel.Debug, $"Loan details of loan of loanId={loan.Id}, for userId={loan.UserId}, was edited successfuly");
                transaction.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                transaction.Rollback();    //Undo DB transactions
                _logger.Log(LogLevel.Error, $"A loan of loanId={loan.Id}, amount={loan.Amount} for userId={loan.UserId} could not be edited, error = {e.Message}", e);

                return View();
            }
        }

        // POST: LoansController/Delete/5
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var loan = await _context.Loans.FirstOrDefaultAsync(u => u.Id == id);
                _context.Remove(loan);

                await _context.SaveChangesAsync();

                _logger.Log(LogLevel.Debug, $"Loan details of loan of loanId={id}, was deleted successfuly");
                transaction.Commit();

                return Json(new { success = true, message = "Delete successful" });
            }
            catch(Exception e)
            {
                transaction.Rollback();    //Undo DB transactions
                _logger.Log(LogLevel.Error, $"A loan of loanId={id} could not be deleted, error = {e.Message}", e);

                return Json(new { success = true, message = "Delete failed" });
            }
        }
    }
}

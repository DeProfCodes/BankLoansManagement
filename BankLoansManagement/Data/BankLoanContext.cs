using BankLoansManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace BankLoansManagement.Data
{
    public class BankLoanContext : DbContext
    {
        public BankLoanContext(DbContextOptions<BankLoanContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}

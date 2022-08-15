using BankLoansManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace BankLoansManagement.ViewModels
{
    public class UserLoanViewModel
    {
        public int LoanId { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Client Firstname")]
        public string ClientFirstName { get; set; }

        [Display(Name = "Client Lastname")]
        public string ClientLastName { get; set; }

        [Display(Name = "Client ID Number")]
        public string ClientIdNumber{ get; set; }

        [Display(Name = "Loan Amount")]
        public double LoanAmount { get; set; }

        [Display(Name = "Loan Type")]
        public string LoanType{ get; set; }

        [Display(Name = "Loan Term")]
        public int LoanTerm { get; set; }

        [Display(Name = "Loan Interest Rate")]
        public double LoanInterestRate { get; set; }

        [Display(Name = "Loan Total Amount")]
        public double LoanTotalAmount { get; set; }
    }
}

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

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        [Display(Name = "Loan Amount")]
        public double LoanAmount { get; set; }

        [Required]
        [Display(Name = "Loan Type")]
        public string LoanType{ get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        [Display(Name = "Loan Term")]
        public int LoanTerm { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        [Display(Name = "Loan Interest Rate")]
        public double LoanInterestRate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        [Display(Name = "Loan Total Amount")]
        public double LoanTotalAmount { get; set; }
    }
}

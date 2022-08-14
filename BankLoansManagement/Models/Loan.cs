using System.ComponentModel.DataAnnotations;

namespace BankLoansManagement.Models
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public int Term { get; set; }
        public double InterestRate { get; set; }
        public string Type { get; set; }
        public double Total { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BankLoansManagement.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Display(Name = "Firstname")]
        public string FirstName { get; set; }
        
        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Display(Name = "ID Number")]
        public string IdNumber { get; set; }
    }
}

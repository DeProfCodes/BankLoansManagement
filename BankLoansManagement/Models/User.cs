using System.ComponentModel.DataAnnotations;

namespace BankLoansManagement.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Firstname")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Required]
        [MinLength(13, ErrorMessage ="ID Number Too Short")]
        [MaxLength(13, ErrorMessage = "ID Number Too Long")]
        [Display(Name = "ID Number")]
        public string IdNumber { get; set; }
    }
}

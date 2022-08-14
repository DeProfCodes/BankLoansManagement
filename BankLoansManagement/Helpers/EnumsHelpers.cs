using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace BankLoansManagement.Helpers
{
    public class EnumsHelpers
    {
        public enum LoanType
        {
            [Display(Name = "Personal Loan")]
            Personal,

            [Display(Name = "Home Loan")]
            Home,

            [Display(Name = "Vehicle Loan")]
            Vehicle
        };

        // Helper method to display the name of the enum values.
        public static string GetDisplayName(Enum value)
        {
            return value.GetType()?
           .GetMember(value.ToString())?.First()?
           .GetCustomAttribute<DisplayAttribute>()?
           .Name;
        }
    }
}

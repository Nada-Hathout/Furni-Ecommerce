using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni_Ecommerce_Shared.AdminViewModel
{
    public class AllUserViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Roles")]
        public List<string> Roles { get; set; } = new List<string>();
    }
}

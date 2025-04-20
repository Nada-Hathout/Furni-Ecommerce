using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni_Ecommerce_Shared.AdminViewModel
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(100, ErrorMessage = "Role name must be between {2} and {1} characters.", MinimumLength = 2)]
        public string RoleName { get; set; }
    }
}

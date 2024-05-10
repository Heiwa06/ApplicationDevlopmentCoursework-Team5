using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Bislerium.RequestedViewModel
{
    public class ViewAuthenticationModel
    {
        public class RegisterModel
        {
            [Required]
            public string? Name { get; set; }
            [Required]
            public string? Phone { get; set; }
            [Required]
            public string? Gender { get; set; }
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string? ConfirmPassword { get; set; }

            [Required]
            public string? Role { get; set; }
        }

        public class LoginModel
        {
            [Required]
            public string? Email { get; set; }
            [Required]
            public string? Password { get; set; }
        }

        public class UpdateModel
        {
            [Required]
            public string? Name { get; set; }
            public string? Phone { get; set; }
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

        }

        public class ChangePasswordModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Old Password")]
            public string OldPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            public string NewPassword { get; set; }

            /*[DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]*/
            public string ConfirmNewPassword { get; set; }
        }


    }
}

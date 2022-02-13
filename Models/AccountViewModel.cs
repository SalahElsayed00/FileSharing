using FileSharing.Resources;
using System.ComponentModel.DataAnnotations;

namespace FileSharing.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Emaillabel", ResourceType = typeof(SharedResource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "passwordlabel", ResourceType = typeof(SharedResource))]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Emaillabel", ResourceType = typeof(SharedResource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "passwordlabel", ResourceType = typeof(SharedResource))]
        public string Password { get; set; }

        [Compare("Password")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "confirmpasswordlabel", ResourceType = typeof(SharedResource))]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required, DataType(DataType.Password), Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Confirm New Password"), Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }

    public class AddPasswordViewModel
    {
        [Required, DataType(DataType.Password), Display(Name = "Local Password")]
        public string Password { get; set; }
    }
}

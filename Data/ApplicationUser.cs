using FileSharing.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace FileSharing.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool IsBlocked { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public DateTime CreateDate { get; set; }
    }
}

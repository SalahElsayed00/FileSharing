using System;
using System.ComponentModel.DataAnnotations;

namespace FileSharing.Areas.Admin.Models
{
    public class AdminUserViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace FileSharing.Areas.Admin.Models
{
    public class TermViewModel
    {
        [MinLength(3), Required]
        public string Term { get; set; }
    }
}

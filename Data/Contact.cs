using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileSharing.Data
{
    public class Contact
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public bool Status { get; set; }

        public DateTime SentDate { get; set; }

        public string Phone { get; set; }

        public string Messege { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}

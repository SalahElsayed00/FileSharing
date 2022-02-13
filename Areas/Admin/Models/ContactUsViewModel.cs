using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharing.Areas.Admin.Models
{
    public class ContactUsViewModel 
    {
        public string Id { get; set; }

        public string Name { get; set; }


        public string Email { get; set; }


        public string Subject { get; set; }


        public string Phone { get; set; }


        public string Messege { get; set; }

        public bool Status { get; set; }

        public DateTime SentDate { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FileSharing.Models
{
    public class InputUpload
    {
        public IFormFile File { get; set; }
    }

    public class UploadViewModel
    {
        public string Id { get; set; }
        [Display(Name = "File Name")]
        public string FileName { get; set; }
        [Display(Name = "Name")]
        public string OriginalName { get; set; }
        [Display(Name = "Size")]
        public decimal Size { get; set; }
        [Display(Name = "Content Type")]
        public string ContentType { get; set; }
        [Display(Name = "DownLoads Count")]
        public long DownloadCount { get; set; }
        [Display(Name = "Upload Date")]
        public DateTime UploadDate { get; set; }

    }
}

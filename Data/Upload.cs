using System;

namespace FileSharing.Data
{
    public class Upload
    {
        public Upload()
        {
            Id = Guid.NewGuid().ToString();
            UploadDate = DateTime.Now;
        }
        public string Id { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
        public decimal Size { get; set; }
        public DateTime UploadDate { get; set; }
        public long DownloadCount { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

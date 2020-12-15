using System;

namespace UploadImage.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}

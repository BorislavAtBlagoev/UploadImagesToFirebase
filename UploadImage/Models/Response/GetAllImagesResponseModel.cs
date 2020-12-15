using System;

namespace UploadImage.Models.Response
{
    public class GetAllImagesResponseModel
    {
        public Guid ImageId { get; set; }
        public string ImageUrl { get; set; }
    }
}

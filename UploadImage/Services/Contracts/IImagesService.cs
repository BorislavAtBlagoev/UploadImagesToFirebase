using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UploadImage.Models;
using UploadImage.Models.Response;

namespace UploadImage.Services.Contracts
{
    public interface IImagesService
    {
        Task<IEnumerable<GetAllImagesResponseModel>> GetAllByUser(string userId);
        Task<ImageResponseModel> GetById(Guid imageId, string userId);
        Task Upload(ICollection<IFormFile> files, string userId);
        Task Delete(Guid imageId, string userId);
    }
}

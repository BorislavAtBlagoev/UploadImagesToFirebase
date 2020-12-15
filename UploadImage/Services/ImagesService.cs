using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UploadImage.Data;
using UploadImage.Models;
using UploadImage.Models.Response;
using UploadImage.Services.Contracts;
using UploadImage.Infrasructures;

namespace UploadImage.Services
{
    public class ImagesService : IImagesService
    {
        private readonly ImageDbContext db;

        public ImagesService(ImageDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<GetAllImagesResponseModel>> GetAllByUser(string userId)
            => await this.db.Images
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .Select(x => new GetAllImagesResponseModel
                {
                    ImageUrl = x.ImageUrl,
                    ImageId = x.Id
                })
                .ToArrayAsync();
        public async Task<ImageResponseModel> GetById(Guid imageId, string userId)
            => await this.db.Images
            .Where(x => x.Id == imageId && x.UserId == userId)
            .Select(x => new ImageResponseModel
            {
                Id = x.Id,
                ImageUrl = x.ImageUrl
            })
            .FirstOrDefaultAsync();
        public async Task Upload(ICollection<IFormFile> files, string userId)
        {
            var filePaths = new HashSet<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.
                    filePaths.Add(filePath);

                    using (var streamFile = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(streamFile);
                    }
                }
            }

            foreach (var filePath in filePaths)
            {
                using var stream = File.Open(filePath, FileMode.Open);

                // of course you can login using other method, not just email+password
                var auth = new FirebaseAuthProvider(new FirebaseConfig(WebConst.ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(WebConst.AuthEmail, WebConst.AuthPassword);

                var task = new FirebaseStorage(
                    WebConst.Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    })
                    .Child("UploadImagesFolder")
                    .Child(filePath.Substring(filePath.Length - 10, 10))
                    .PutAsync(stream);

                var imageUrl = await task;

                var image = new Image
                {
                    ImageUrl = imageUrl,
                    IsDeleted = false,
                    UserId = userId
                };

                await this.db.Images.AddAsync(image);
            }

            await this.db.SaveChangesAsync();
        }
        public async Task Delete(Guid imageId, string userId)
        {
            var image = await this.GetByIdAndUserId(imageId, userId);

            image.IsDeleted = true;
            await this.db.SaveChangesAsync();
        }

        private async Task<Image> GetByIdAndUserId(Guid pictureId, string userId)
            => await this.db
                .Images
                .Where(x => x.Id == pictureId && x.UserId == userId)
                .FirstOrDefaultAsync();
    }
}

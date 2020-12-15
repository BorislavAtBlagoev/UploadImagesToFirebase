using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UploadImage.Infrasructures.Extentions;
using UploadImage.Models.Request;
using UploadImage.Services.Contracts;

namespace UploadImage.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private readonly IImagesService imagesService;

        public ImageController(IImagesService imagesService) => this.imagesService = imagesService;

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var userId = this.User.GetId();

            var images = await this.imagesService.GetAllByUser(userId);

            return View(images);
        }

        [HttpGet]
        public async Task<IActionResult> Upload() => await Task.Run(() => View());

        [HttpPost]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            var userId = this.User.GetId();

            await this.imagesService.Upload(files, userId);

            return RedirectToAction("All", "Image");
        }

        [HttpGet]
        public async Task<IActionResult> Delete() => await Task.Run(() => View());

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteImageRequestModel image)
        {
            var userId = this.User.GetId();

            await this.imagesService.Delete(image.Id, userId);

            return RedirectToAction("All", "Image");
        }

        [HttpGet]
        public async Task<IActionResult> Details(DetailsImageRequestModel model)
        {
            var userId = this.User.GetId();

            var image = await this.imagesService
                .GetById(model.Id, userId);

            return View(image);
        }
    }
}


using E_Commerce_Mezzex.Models.Domain;
using E_Commerce_Mezzex.Models.ViewModel;
using E_Commerce_Mezzex.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Mezzex.Controllers
{
    [Authorize(Roles = "Admin,Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesDetailsController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesDetailsController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SaveAllAsync([FromBody] List<ImageViewModel> images)
        {
            if (images == null || images.Count == 0)
            {
                return BadRequest("No image details received");
            }

            try
            {
                foreach (var image in images)
                {
                    var picture = new Picture
                    {
                        VirtualPath = image.VirtualPath,
                        ProductId = image.ProductId,
                        SeoFilename = image.SeoFilename,
                        AltAttribute = image.AltAttribute,
                        TitleAttribute = image.TitleAttribute,
                        MediaType = image.MediaType
                    };

                    await _imageRepository.AddAsync(picture);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception (ensure you have a logging mechanism)
                Console.Error.WriteLine(ex);
                return StatusCode(500, "An error occurred while saving the images.");
            }
        }
    }
}

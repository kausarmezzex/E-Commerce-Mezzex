using E_Commerce_Mezzex.Models.Domain;
using E_Commerce_Mezzex.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace E_Commerce_Mezzex.Controllers
{
    [Authorize(Roles = "Admin,Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file, int productId, MediaType mediaType)
        {
            var mediaUrl = await _imageRepository.UploadAsync(file, productId, mediaType, null, null, null);
            if (mediaUrl == null)
            {
                return Problem("Something went wrong during media upload.", null, (int)HttpStatusCode.InternalServerError);
            }

            return new JsonResult(new { link = mediaUrl });
        }
    }
}
